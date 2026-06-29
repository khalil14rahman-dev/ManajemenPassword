using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private MySqlConnection _connection;

        private readonly string connectionString = "Server=localhost;Database=password_manager;Uid=root;Pwd=;";

        private DatabaseConnection()
        {
            _connection = new MySqlConnection(connectionString);
        }

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

        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    _connection.Open();
                }
                catch (MySqlException ex) 
                {
                    throw new Exception("Gagal terhubung ke database server: " + ex.Message);
                }
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}