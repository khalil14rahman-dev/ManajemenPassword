using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Project_KPL_ManajemenPassword
{
    // [CLEAN CODE] - Penamaan class mencerminkan fungsinya dengan jelas
    public class DataRepository<T> where T : class
    {
        private readonly string _filePath;

        // DESIGN PATTERN: SINGLETON
        private static DataRepository<T> _instance;
        private static readonly object _lock = new object();

        private DataRepository(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path file tidak boleh kosong!", nameof(path));
            }
            _filePath = path;
        }

        // Method global untuk akses instance tunggal (Singleton Gate)
        public static DataRepository<T> GetInstance(string path)
        {
            if (_instance == null)
            {
                lock (_lock) 
                {
                    if (_instance == null)
                    {
                        _instance = new DataRepository<T>(path);
                    }
                }
            }
            return _instance;
        }

        public void SaveData(List<T> dataList)
        {
            if (dataList == null)
            {
                throw new ArgumentNullException(nameof(dataList), "Data tidak boleh null!");
            }

            try
            {
                string jsonString = JsonSerializer.Serialize(dataList);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gagal menulis data ke JSON.", ex);
            }
        }

        public List<T> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<T>();

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}