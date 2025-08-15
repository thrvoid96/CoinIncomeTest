using System;

namespace Runtime.Datas
{
    [Serializable]
    public sealed class CoinSaveData
    {
        public int Coins = 0;
        public int Income = 1;
        public int UpgradeLevel = 0;
    }
}