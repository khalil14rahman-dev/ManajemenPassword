using System;
using System.Text;

namespace Project_KPL_ManajemenPassword
{
    public static class SecurityService
    {
        /// <param name="plainText"
 
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }


        /// <param name="base64EncodedData">
        public static string Decrypt(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData)) return "";

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
