using System.Threading.Tasks;

namespace Runtime.Interfaces
{
    public interface ISaveManager
    {
        Task SaveAsync<T>(string key, T data);
        Task<T> LoadAsync<T>(string key) where T : class, new();
        Task DeleteAsync(string key);
        void Save<T>(string key, T data);
        void Delete(string key);
    }
}