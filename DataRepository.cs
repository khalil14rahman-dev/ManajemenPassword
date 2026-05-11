using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Project_KPL_ManajemenPassword
{
    public class DataRepository<T> where T : class
    {
        private string filePath;

        public DataRepository(string fileName)
        { 
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        // Simpan (Generic)
        public void SaveData(List<T> dataList)
        {
            string jsonString = JsonSerializer.Serialize(dataList);
            File.WriteAllText(filePath, jsonString);
        }

        // Ambil Data (Generic)
       public List<T> LoadData()
        {
            if (!File.Exists(filePath)) return new List<T>();

            try 
            {
                string jsonString = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<List<T>>(jsonString);

                return result ?? new List<T>();
            }
            catch 
            {
                return new List<T>();
            }
        }
    }
}
