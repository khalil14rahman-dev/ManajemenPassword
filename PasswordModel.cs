using System;

namespace Project_KPL_ManajemenPassword
{
    public class PasswordModel
    {

        public int IdPassword { get; set; }
        public int IdUser { get; set; }
        public int IdCategory { get; set; }
        public string NamaAplikasi { get; set; }

        public string UsernameAkun { get; set; }
        public string Password { get; set; }

        public byte IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }

      
        public PasswordModel() { }

        public PasswordModel(string aplikasi, string user, string pass)
        {
            this.NamaAplikasi = aplikasi;
            this.UsernameAkun = user;
            this.Password = pass;
            this.IsActive = 1; 
        }

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