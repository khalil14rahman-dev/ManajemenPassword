using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class PasswordModel
    {
        public string NamaAplikasi { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public PasswordModel() { }

        public PasswordModel(string aplikasi, string user, string pass)
        {
            this.NamaAplikasi = aplikasi;
            this.Username = user;
            this.Password = pass;
        }

    //BUAT KEBUTUHAN UNIT TEST
    public static string GeneratePassword()
        {
            string[] karakterTabel = {
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        "abcdefghijklmnopqrstuvwxyz",
        "1234567890",
        "!@#$%^&*"
    };

            Random rand = new Random();
            string passwordBaru = "";
            for (int i = 0; i < karakterTabel.Length; i++)
            {
                string barisKarakter = karakterTabel[i];
                for (int j = 0; j < 2; j++)
                {
                    passwordBaru += barisKarakter[rand.Next(barisKarakter.Length)];
                }
            }
            return passwordBaru;
        }
    }
}
