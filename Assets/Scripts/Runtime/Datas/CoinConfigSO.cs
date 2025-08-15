using UnityEngine;

namespace Runtime.Datas
{
    [CreateAssetMenu(fileName = "CoinConfig", menuName = "Game/Coin Config", order = 0)]
    public sealed class CoinConfigSO : ScriptableObject
    {
        [Header("Initial Settings")]
        public int startingCoins = 0;
        public int startingIncome = 1;

        [Header("Passive Income Settings")]
        public float incomeIntervalSeconds = 1f;

        [Header("Upgrade Settings")]
        public int upgradeBaseCost = 50;

        [Tooltip("Formula multiplier for income upgrade. Changeable anytime.")]
        public float incomeMultiplier = 2f;

        [Tooltip("Formula multiplier for price increase after upgrade.")]
        public float costMultiplier = 1.5f;

        public int CalculateUpgradeCost(int currentLevel) =>
            Mathf.RoundToInt(upgradeBaseCost * Mathf.Pow(costMultiplier, currentLevel));

        public int CalculateNewIncome(int currentIncome) =>
            Mathf.RoundToInt(currentIncome * incomeMultiplier);
    }
}