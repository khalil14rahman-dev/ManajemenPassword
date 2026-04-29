using System;
using System.Text;

namespace Project_KPL_ManajemenPassword
{
    public static class SecurityService
    {
        /// <summary>
        /// Fungsi untuk mengenkripsi (mengacak) teks agar tidak bisa dibaca langsung di file JSON.
        /// </summary>
        /// <param name="plainText">Password asli yang diinput user</param>
        /// <returns>Teks yang sudah diacak dalam format Base64</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";

            // Mengubah string menjadi byte array
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Mengubah byte array menjadi string Base64 (Enkripsi sederhana)
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Fungsi untuk mendekripsi (mengembalikan) teks yang diacak menjadi password asli.
        /// </summary>
        /// <param name="base64EncodedData">Teks acak dari file JSON</param>
        /// <returns>Password asli yang bisa dibaca user</returns>
        public static string Decrypt(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData)) return "";

            // Mengubah string Base64 kembali menjadi byte array
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            // Mengubah byte array kembali menjadi string teks biasa
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}