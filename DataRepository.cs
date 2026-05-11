using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

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
            Debug.Assert(!string.IsNullOrEmpty(filePath), "FilePath tidak boleh kosong!");

            if (!File.Exists(filePath)) return new List<T>();

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var result = JsonSerializer.Deserialize<List<T>>(jsonString);

                List<T> finalResult = result ?? new List<T>();

                Debug.Assert(finalResult != null, "Post-condition gagal: Output tidak boleh null!");

                return finalResult;
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}
