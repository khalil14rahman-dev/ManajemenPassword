using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Project_KPL_ManajemenPassword
{
    public class DataRepository<T>
    {
        private string filePath;

        public DataRepository(string fileName)
        {
            // Lokasi file akan ada di folder bin/Debug aplikasi
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        // Fungsi Simpan (Generic)
        public void SaveData(List<T> dataList)
        {
            string jsonString = JsonSerializer.Serialize(dataList);
            File.WriteAllText(filePath, jsonString);
        }

        // Fungsi Ambil Data (Generic)
        public List<T> LoadData()
        {
            if (!File.Exists(filePath)) return new List<T>();

            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}
