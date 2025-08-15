using System.Threading.Tasks;
using Runtime.Datas;
using Runtime.Interfaces;
using Runtime.UI.CoinUI;
using UnityEngine;

namespace Runtime.Managers
{
    public sealed class CoinGameInstaller : MonoBehaviour
    {
        [field : SerializeField] public CoinConfigSO config { get; private set; }
        [SerializeField] private CoinUIView view;
        
        public (int coins, int income) startingValues
        {
            get
            {
                if (config == null) throw new System.NullReferenceException("CoinConfigSO is not assigned.");
                return (config.startingCoins, config.startingIncome);
            }
        }


        private CoinModel _model;
        private CoinController _controller;
        private ICoinRepository _repository;

        private async void Start()
        {
            var saveManager = new JsonSaveManager();
            _repository = new JsonCoinRepository(saveManager);

            _model = new CoinModel();
            _controller = new CoinController(_model, config, _repository);
            
            _model.OnCoinsChanged += view.SetCoins;
            _model.OnIncomeChanged += income =>
            {
                view.SetIncome(income);
                view.UpdateUpgradeCost(config.CalculateUpgradeCost(_model.UpgradeLevel));
            };

            view.OnCollectClicked += async () => await _controller.CollectAsync();
            view.OnUpgradeClicked += async () =>
            {
                if (await _controller.UpgradeAsync())
                    view.UpdateUpgradeCost(config.CalculateUpgradeCost(_model.UpgradeLevel));
            };

            await _controller.LoadAsync(config);
            view.Init(_model.Coins, _model.Income, config.CalculateUpgradeCost(_model.UpgradeLevel));

            _controller.StartPassiveIncome();
        }

        private async void OnApplicationQuit()
        {
            _controller.StopPassiveIncome();
            await _repository.SaveAsync(new CoinSaveData
            {
                Coins = _model.Coins,
                Income = _model.Income,
                UpgradeLevel = _model.UpgradeLevel
            });
        }

        public async Task ClearDataAsync()
        {
            if (_repository != null)
            {
                await _repository.SaveAsync(new CoinSaveData
                {
                    Coins = config.startingCoins,
                    Income = config.startingIncome,
                    UpgradeLevel = 0
                });
            }
            _model?.Initialize(config.startingCoins, config.startingIncome, 0);
        }

        public void ClearDataImmediate()
        {
            if (_repository != null)
            {
                _repository.SaveAsync(new CoinSaveData
                {
                    Coins = config.startingCoins,
                    Income = config.startingIncome,
                    UpgradeLevel = 0
                }).Wait();
            }
        }
    }
}
