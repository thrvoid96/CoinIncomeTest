using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.CoinUI
{
    public sealed class CoinUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text incomeText;
        [SerializeField] private Button collectButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeCostText;

        public event System.Action OnCollectClicked;
        public event System.Action OnUpgradeClicked;

        public void Init(int coins, int income, int upgradeCost)
        {
            SetCoins(coins);
            SetIncome(income);
            upgradeCostText.text = $"Upgrade ({upgradeCost})";
        }

        public void SetCoins(int value) => coinText.text = $"Coins: {value}";
        public void SetIncome(int value) => incomeText.text = $"Income: {value}";

        private void Awake()
        {
            collectButton.onClick.AddListener(() => OnCollectClicked?.Invoke());
            upgradeButton.onClick.AddListener(() => OnUpgradeClicked?.Invoke());
        }

        public void UpdateUpgradeCost(int cost)
        {
            upgradeCostText.text = $"Upgrade ({cost})";
        }
    }
}