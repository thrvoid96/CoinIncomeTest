using System.Threading.Tasks;
using Runtime.Constants;
using Runtime.Datas;
using Runtime.Interfaces;

namespace Runtime.Managers
{
    public sealed class JsonCoinRepository : ICoinRepository
    {
        private readonly ISaveManager _saveManager;
        
        public JsonCoinRepository(ISaveManager saveManager)
        {
            _saveManager = saveManager ?? throw new System.ArgumentNullException(nameof(saveManager));
        }

        public Task<CoinSaveData> LoadAsync()
            => _saveManager.LoadAsync<CoinSaveData>(GameConstant.CoinSaveKey);

        public Task SaveAsync(CoinSaveData data)
            => _saveManager.SaveAsync(GameConstant.CoinSaveKey, data);

        public Task DeleteAsync()
            => _saveManager.DeleteAsync(GameConstant.CoinSaveKey); // Now part of interface
        
        public void Save(CoinSaveData data)
            => _saveManager.Save(GameConstant.CoinSaveKey, data); // Now part of interface
        
        public void Delete()
            => _saveManager.Delete(GameConstant.CoinSaveKey); // Now part of interface
    }
}