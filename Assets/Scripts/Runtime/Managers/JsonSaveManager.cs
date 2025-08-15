using System.IO;
using System.Threading.Tasks;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.Managers
{
    public sealed class JsonSaveManager : ISaveManager
    {
        private readonly string _saveDirectory = Application.persistentDataPath;

        public async Task SaveAsync<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            var path = Path.Combine(_saveDirectory, key + ".json");
            await File.WriteAllTextAsync(path, json);
        }

        public async Task<T> LoadAsync<T>(string key) where T : class, new()
        {
            var path = Path.Combine(_saveDirectory, key + ".json");
            if (!File.Exists(path)) return null;

            var json = await File.ReadAllTextAsync(path);
            return JsonUtility.FromJson<T>(json);
        }
        
        public Task DeleteAsync(string key)
        {
            var path = Path.Combine(_saveDirectory, key + ".json");
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Save file deleted: {path}");
            }
            return Task.CompletedTask;
        }
        
        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            var path = Path.Combine(Application.persistentDataPath, key + ".json");
            File.WriteAllText(path, json);
        }
        
        public void Delete(string key)
        {
            var path = Path.Combine(_saveDirectory, key + ".json");
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Save file deleted: {path}");
            }
        }
    }
}