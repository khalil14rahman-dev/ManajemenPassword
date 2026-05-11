using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public static class Security
    {
        // 1. Hashing (Untuk Master Password)
        public static string Hash(string password)
        {
            Debug.Assert(password != null, "Kontrak dilanggar: Password tidak boleh null");
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // 2. Encrypt (Tambahkan ini agar error CS0117 hilang)
        public static string Encrypt(string plainText)
        {
            Debug.Assert(plainText != null, "Kontrak: Plaintext tidak boleh null");
            if (string.IsNullOrEmpty(plainText)) return "";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        // 3. Decrypt (Tambahkan ini agar error CS0117 hilang)
        public static string Decrypt(string base64Data)
        {
            if (string.IsNullOrEmpty(base64Data)) return "";
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64Data));
        }
    }
}