using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public class DataRepository<T> where T : class
    {
        //design pattern instance tunggal bersifat private dan static
        private static DataRepository<T> _instance;
        private static readonly object _lock = new object();

        private string filePath;

        //design pattern, constructor diubah menjadi private agar tidak bisa di- "new" sembarangan dari luar
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

            filePath = fileName;
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
        public void SaveData(List<T> dataList)
        {
            // dbc pre kondisi
            Debug.Assert(dataList != null, "Kontrak Gagal: List data tidak boleh null saat akan disimpan.");

            if (dataList == null)
            {
                throw new ArgumentNullException(nameof(dataList), "DbC Violation [Pre-condition]: List data tidak boleh null saat akan disimpan!");
            }

            try
            {
                string jsonString = JsonSerializer.Serialize(dataList);
                File.WriteAllText(filePath, jsonString);

                // dbc post kondisi
                Debug.Assert(File.Exists(filePath), "Kontrak Gagal: File data tidak ditemukan setelah proses simpan.");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("DbC Violation [Post-condition]: File data tidak ditemukan setelah proses simpan!");
                }
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

            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("DbC Violation [Pre-condition]: FilePath tidak boleh kosong!");
            }

            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<T>();
                }

                string jsonString = File.ReadAllText(filePath);
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