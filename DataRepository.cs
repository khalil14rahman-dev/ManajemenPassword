using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace Project_KPL_ManajemenPassword
{
    public class DataRepository
    {
        private static DataRepository _instance;
        private static readonly object _lock = new object();

        private DataRepository()
        {
            try
            {
                DatabaseConnection.GetInstance().OpenConnection();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("DbC Violation [Pre-condition]: Gagal inisialisasi koneksi database. " + ex.Message);
            }
            finally
            {
                DatabaseConnection.GetInstance().CloseConnection();
            }
        }

        public static DataRepository GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DataRepository();
                }
                return _instance;
            }
        }

        public event Action OnDataChanged;

        public List<PasswordModel> LoadData()
        {
            List<PasswordModel> listResult = new List<PasswordModel>();
            DatabaseConnection db = DatabaseConnection.GetInstance();

            int currentUserId = AuthManager.GetInstance().CurrentIdUser;

            try
            {
                db.OpenConnection();
                string query = "SELECT * FROM password_model WHERE is_active = 1 AND id_user = @id_user";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@id_user", currentUserId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PasswordModel item = new PasswordModel
                        {
                            IdPassword = Convert.ToInt32(reader["id_password"]),
                            IdCategory = Convert.ToInt32(reader["id_category"]),
                            NamaAplikasi = reader["nama_aplikasi"].ToString(),
                            UsernameAkun = reader["username_akun"].ToString(),
                            Password = reader["password"].ToString()
                        };
                        listResult.Add(item);
                    }
                }

                Debug.Assert(listResult != null, "Post-condition gagal: Output list tidak boleh null!");
                return listResult;
            }
            catch
            {
                return new List<PasswordModel>();
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public void SaveData(PasswordModel data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            DatabaseConnection db = DatabaseConnection.GetInstance();

            int currentUserId = AuthManager.GetInstance().CurrentIdUser;

            try
            {
                db.OpenConnection();
                string query;

                if (data.IdPassword == 0)
                {
                    query = @"INSERT INTO password_model (id_user, id_category, nama_aplikasi, username_akun, password, is_active) 
                             VALUES (@id_user, @id_category, @nama_aplikasi, @username_akun, @password, 1)";
                }
                else
                {
                    query = @"UPDATE password_model 
                             SET id_category = @id_category, nama_aplikasi = @nama_aplikasi, username_akun = @username_akun, password = @password 
                             WHERE id_password = @id_password AND id_user = @id_user";
                }

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());

                if (data.IdPassword > 0) cmd.Parameters.AddWithValue("@id_password", data.IdPassword);
                cmd.Parameters.AddWithValue("@id_user", currentUserId);
                cmd.Parameters.AddWithValue("@id_category", data.IdCategory);
                cmd.Parameters.AddWithValue("@nama_aplikasi", data.NamaAplikasi);
                cmd.Parameters.AddWithValue("@username_akun", data.UsernameAkun);
                cmd.Parameters.AddWithValue("@password", data.Password);

                cmd.ExecuteNonQuery();

                OnDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gagal mengeksekusi operasi simpan ke database MySQL. Detail: " + ex.Message, ex);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public void SoftDeleteData(int idPassword)
        {
            DatabaseConnection db = DatabaseConnection.GetInstance();

            int currentUserId = AuthManager.GetInstance().CurrentIdUser;

            try
            {
                db.OpenConnection();

                string query = "UPDATE password_model SET is_active = 0 WHERE id_password = @id_password AND id_user = @id_user";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@id_password", idPassword);
                cmd.Parameters.AddWithValue("@id_user", currentUserId); // 🛠️ TAMBAHAN

                cmd.ExecuteNonQuery();

                OnDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gagal melakukan penghapusan data di database MySQL.", ex);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public Dictionary<int, string> GetCategories()
        {
            Dictionary<int, string> categories = new Dictionary<int, string>();
            DatabaseConnection db = DatabaseConnection.GetInstance();

            try
            {
                db.OpenConnection();
                string query = "SELECT id_category, nama_kategori FROM categories";

                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id_category"]);
                        string nama = reader["nama_kategori"].ToString();
                        categories.Add(id, nama);
                    }
                }
                return categories;
            }
            catch
            {
                categories.Add(1, "Umum / Default");
                return categories;
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public Dictionary<int, string> GetSecurityQuestions()
        {
            Dictionary<int, string> questions = new Dictionary<int, string>();
            DatabaseConnection db = DatabaseConnection.GetInstance();

            try
            {
                db.OpenConnection();
                string query = "SELECT id_question, text_question FROM security_questions";
                MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id_question"]);
                        string text = reader["text_question"].ToString();
                        questions.Add(id, text);
                    }
                }
                return questions;
            }
            catch
            {
                questions.Add(1, "Apa nama sekolah dasar Anda?");
                return questions;
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}