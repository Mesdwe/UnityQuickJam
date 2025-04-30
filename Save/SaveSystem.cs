using System.IO;
using UnityEngine;
using QuickJam.Core;

namespace QuickJam.Save
{
    public class SaveSystem : Singleton<SaveSystem>
    {
        private string _saveDirectory => Path.Combine(Application.persistentDataPath, "Saves");

        public string GetSavePath(string fileName)
        {
            return Path.Combine(_saveDirectory, $"{fileName}.json");
        }

        public bool Exists(string fileName)
        {
            return File.Exists(GetSavePath(fileName));
        }

        public void Save<T>(string fileName, T data)
        {
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetSavePath(fileName), json);

            Debug.Log($"[SaveSystem] Saved {fileName} to {_saveDirectory}");
        }

        public T Load<T>(string fileName) where T : new()
        {
            string path = GetSavePath(fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SaveSystem] Save file not found: {fileName}, returning new default instance.");
                return new T();
            }

            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            Debug.Log($"[SaveSystem] Loaded {fileName}");
            return data;
        }

        public void Delete(string fileName)
        {
            string path = GetSavePath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[SaveSystem] Deleted {fileName}");
            }
        }
    }
}