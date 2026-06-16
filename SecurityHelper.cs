using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            //Guard Clause (CWE-20: Improper Input Validation)
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password yang akan di-enkripsi tidak valid.");
            }

            //automatic resource disposal (cegah celah kebocoran memori (memory leak))
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
    }
}
