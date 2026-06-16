using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private MySqlConnection _connection;

        // Konfigurasi String Koneksi (Gaya penulisan string tetap sama persis untuk phpMyAdmin)
        private readonly string connectionString = "Server=localhost;Database=password_manager_db;Uid=root;Pwd=;";

        // Constructor bertipe private (Singleton)
        private DatabaseConnection()
        {
            // Tetap menggunakan kelas MySqlConnection bawaan dari MySqlConnector
            _connection = new MySqlConnection(connectionString);
        }

        // Gerbang Singleton Pattern
        public static DatabaseConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection();
            }
            return _instance;
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        // Metode penunjang membuka koneksi secara aman
        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    _connection.Open();
                }
                catch (MySqlException ex) // Menangkap exception spesifik dari MySqlConnector
                {
                    throw new Exception("Gagal terhubung ke database server via MySqlConnector: " + ex.Message);
                }
            }
        }

        // Metode penunjang menutup koneksi
        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
