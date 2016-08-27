using System;
using System.Text;
using System.Security.Cryptography;

namespace OMInsurance.Utils
{
    public class PasswordHash
    {
        static string salt = "AwmvIzC1i2Je5OBe";

        public static string CreateHash(string password)
        {
            string hash = ComputeHash(password, salt);
            return hash;
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            string testHash = ComputeHash(password, salt);
            return correctHash.Equals(testHash);
        }

        private static string ComputeHash(string password, string salt)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
