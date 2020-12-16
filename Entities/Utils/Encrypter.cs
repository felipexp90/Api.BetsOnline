using System;
using System.Security.Cryptography;
using System.Text;

namespace Entities.Utils
{
    public static class Encrypter
    {
        public static string EncryptString (string value, string secret)
        {
            byte[] key;
            byte[] array = UTF8Encoding.UTF8.GetBytes(value);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(secret));
            md5.Clear();
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform convert = tripledes.CreateEncryptor();
            byte[] result = convert.TransformFinalBlock(array, 0, array.Length);
            tripledes.Clear();
            return Convert.ToBase64String(result, 0, result.Length);
        }

        public static string DecryptString(string value, string secret)
        {
            byte[] key;
            byte[] array = Convert.FromBase64String(value);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(secret));
            md5.Clear();
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform convert = tripledes.CreateDecryptor();
            byte[] result = convert.TransformFinalBlock(array, 0, array.Length);
            tripledes.Clear();
            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}