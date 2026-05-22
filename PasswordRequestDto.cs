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

        // PENGGANTI DEFENSIVE PROGRAMMING MANUAL:
        // Semua pengecekan yang tadinya ada di Form1 dan AuthManager disatukan di fungsi ini.
        public bool IsValid()
        {
            // 1. Cek apakah kosong atau spasi saja (Tadinya ada di Form1 & AuthManager)
            if (string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            // 2. Cek apakah panjangnya kurang dari 8 karakter (Tadinya ada di Form1 & AuthManager)
            if (Password.Length < 8)
            {
                return false;
            }

            // Jika lolos semua pengecekan di atas, berarti data valid
            return true;
        }
    }
}
