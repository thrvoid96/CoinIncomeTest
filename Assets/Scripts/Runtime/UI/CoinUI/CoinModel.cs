using System;
using Runtime.Datas;
using UnityEngine;

namespace Runtime.UI.CoinUI
{
    public sealed class CoinModel
    {
        public int Coins { get; private set; }
        public int Income { get; private set; }
        public int UpgradeLevel { get; private set; }

        public event Action<int> OnCoinsChanged;
        public event Action<int> OnIncomeChanged;

        public void Initialize(int coins, int income, int upgradeLevel)
        {
            Coins = coins;
            Income = income;
            UpgradeLevel = upgradeLevel;
            OnCoinsChanged?.Invoke(Coins);
            OnIncomeChanged?.Invoke(Income);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
        }

        public void UpgradeIncome(int newIncome)
        {
            Income = newIncome;
            OnIncomeChanged?.Invoke(Income);
        }

        public void SpendCoins(int amount)
        {
            Coins -= amount;
            OnCoinsChanged?.Invoke(Coins);
        }

        public CoinSaveData ToSaveData() => new()
        {
            Coins = Coins,
            Income = Income,
            UpgradeLevel = UpgradeLevel
        };

        public void SetUpgradeLevel(int level) => UpgradeLevel = level;
    }
}