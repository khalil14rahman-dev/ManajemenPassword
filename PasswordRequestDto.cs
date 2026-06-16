using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class PasswordRequestDto
    {
        public string Password { get; set; }
        public string SecurityQuestion { get; set; } 
        public string SecurityAnswer { get; set; }   

        // PENGGANTI DEFENSIVE PROGRAMMING MANUAL:
        // Semua pengecekan yang tadinya ada di Form1 dan AuthManager disatukan di fungsi ini.
        public bool IsValid()
        {
            // Clean Code: Menggunakan KISS untuk meningkatkan code readability
            if (string.IsNullOrWhiteSpace(Password)) return false;
            if (Password.Length < 8) return false;

            return true;
        }
    }
}
