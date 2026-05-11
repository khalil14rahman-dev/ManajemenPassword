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
            if (!string.IsNullOrEmpty(fileName) && !fileName.Contains("\\"))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
            Debug.Assert(!string.IsNullOrEmpty(fileName), "Kontrak Gagal: Path file harus ditentukan.");
            filePath = fileName;
        }

        // Simpan (Generic)
        public void SaveData(List<T> dataList)
        {
            //dbc pre kondisi
            Debug.Assert(dataList != null, "Kontrak Gagal: List data tidak boleh null saat akan disimpan.");

            try
            {
                string jsonString = JsonSerializer.Serialize(dataList);
                File.WriteAllText(filePath, jsonString);

                //dbc post kondisi
                Debug.Assert(File.Exists(filePath), "Kontrak Gagal: File data tidak ditemukan setelah proses simpan.");
            }
            catch (Exception ex)
            {
                throw new Exception("Gagal menyimpan data: " + ex.Message);
            }
        }

        // Ambil Data (Generic)
        public List<T> LoadData()
        {
            Debug.Assert(!string.IsNullOrEmpty(filePath), "FilePath tidak boleh kosong!");
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
