using System.Threading;
using System.Threading.Tasks;
using Runtime.Datas;
using Runtime.Helpers;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.UI.CoinUI
{
    public class CoinController
    {
        private readonly CoinModel _model;
        private readonly CoinConfigSO _config;
        private readonly ICoinRepository _repository;
        private CancellationTokenSource _incomeCTS;

        public CoinController(CoinModel model, CoinConfigSO config, ICoinRepository repository)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
            _config = config ?? throw new System.ArgumentNullException(nameof(config));
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task LoadAsync(CoinConfigSO config)
        {
            var data = await _repository.LoadAsync();
            if (data == null)
            {
                data = new CoinSaveData
                {
                    Coins = config.startingIncome,
                    Income = config.startingIncome,
                    UpgradeLevel = 0
                };
            }

            _model.Initialize(data.Coins, data.Income, data.UpgradeLevel);
        }

        private Task SaveAsync() => _repository.SaveAsync(_model.ToSaveData());

        public void StartPassiveIncome()
        {
            _incomeCTS = new CancellationTokenSource();
            _ = PassiveIncomeLoop(_incomeCTS.Token);
        }

        public void StopPassiveIncome() => _incomeCTS?.Cancel();

        private async Task PassiveIncomeLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay((int)(_config.incomeIntervalSeconds * 1000), token);

                    if (UnityMainThreadDispatcher.isOnMainThread)
                    {
                        _model.AddCoins(_model.Income);
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Enqueue(() => _model.AddCoins(_model.Income));
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        public async Task CollectAsync()
        {
            _model.AddCoins(_model.Income);
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpgradeAsync()
        {
            var cost = _config.CalculateUpgradeCost(_model.UpgradeLevel);
            if (_model.Coins < cost) return false;

            _model.SpendCoins(cost);
            _model.SetUpgradeLevel(_model.UpgradeLevel + 1);
            _model.UpgradeIncome(_config.CalculateNewIncome(_model.Income));

            await SaveAsync().ConfigureAwait(false);
            return true;
        }
    }
}
