using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public class DataRepository<T> where T : class
    {
        private static DataRepository<T> _instance;
        private static readonly object _lock = new object();

        private string _filePath;

        private DataRepository(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && !fileName.Contains("\\"))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
            Debug.Assert(!string.IsNullOrEmpty(fileName), "Kontrak Gagal: Path file harus ditentukan.");

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "DbC Violation [Pre-condition]: Path file harus ditentukan dan tidak boleh kosong!");
            }

            _filePath = fileName;
        }

        // design pattern Thread-Safe Singleton Accessor
        // Fungsi ini yang akan dipanggil dari Form untuk mendapatkan objek repo
        public static DataRepository<T> GetInstance(string fileName)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DataRepository<T>(fileName);
                }
                return _instance;
            }
        }

        // Simpan (Generic)
        public event Action OnDataChanged;
        public void SaveData(List<T> dataList)
        {
            // [Andra] Design by Contract (Pre-Condition Validation)
            if (dataList == null)
            {
                throw new ArgumentNullException(nameof(dataList), "Data tidak boleh null sebelum disimpan!");
            }

            try
            {
                // [Andra] JSON Serialization
                string jsonString = JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonString);

                // [Ariel] Observer Trigger -> ke FormDashboard 
                OnDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gagal menulis data ke file JSON.", ex);
            }
        }

        // Ambil Data (Generic)
        public List<T> LoadData()
        {
            Debug.Assert(!string.IsNullOrEmpty(_filePath), "FilePath tidak boleh kosong!");

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new InvalidOperationException("DbC Violation [Pre-condition]: FilePath tidak boleh kosong!");
            }

            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<T>();
                }

                string jsonString = File.ReadAllText(_filePath);
                var result = JsonSerializer.Deserialize<List<T>>(jsonString);

                List<T> finalResult = result ?? new List<T>();

                Debug.Assert(finalResult != null, "Post-condition gagal: Output tidak boleh null!");

                if (finalResult == null)
                {
                    throw new InvalidOperationException("DbC Violation [Post-condition]: Hasil load mengembalikan null!");
                }

                return finalResult;
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}