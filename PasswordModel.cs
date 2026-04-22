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
    }
}
