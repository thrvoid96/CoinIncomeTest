using System.Threading.Tasks;
using Runtime.Datas;

namespace Runtime.Interfaces
{
    public interface ICoinRepository
    {
        Task<CoinSaveData> LoadAsync();
        Task SaveAsync(CoinSaveData data);
        Task DeleteAsync();
    }
}