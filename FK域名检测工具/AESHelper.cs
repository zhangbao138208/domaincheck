using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FK域名检测工具
{
    public class AesHelper
    {
        public static string AES_KEY = "freeknight";
        public static string AES_IV = "1234567890";
        #region AES
        /// <summary>
        /// 创建算法操作对象
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static RijndaelManaged CreateRijndaelManaged(string secretKey, string iv)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;

            rijndaelManaged.Mode = CipherMode.CBC;

            byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] keyBytes = new byte[16];
            Array.Copy(secretBytes, keyBytes, Math.Min(secretBytes.Length, keyBytes.Length));
            rijndaelManaged.Key = keyBytes;

            if (string.IsNullOrEmpty(iv))
            {
                rijndaelManaged.Mode = CipherMode.ECB;
            }
            else
            {
                rijndaelManaged.Mode = CipherMode.CBC;

                byte[] array = Encoding.UTF8.GetBytes(iv);
                byte[] ivBytes = new byte[keyBytes.Length];
                Array.Copy(array, ivBytes, Math.Min(array.Length, ivBytes.Length));
                rijndaelManaged.IV = ivBytes;
            }
            return rijndaelManaged;
        }
        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secretKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncrypt(string value, string secretKey, string iv)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            using (RijndaelManaged rijndaelManaged = CreateRijndaelManaged(secretKey, iv))
            {
                using (ICryptoTransform iCryptoTransform = rijndaelManaged.CreateEncryptor())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(value);
                    buffer = iCryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
                    //使用hex格式输出数据
                    StringBuilder result = new StringBuilder();
                    foreach (byte b in buffer)
                    {
                        result.AppendFormat("{0:x2}", b);
                    }
                    return result.ToString();
                    //或者使用下面的输出
                    //return BitConverter.ToString(buffer).Replace("-", "").ToLower();
                }
            }
        }
        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="secretKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesDecrypt(string value, string secretKey, string iv)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            using (RijndaelManaged rijndaelManaged = CreateRijndaelManaged(secretKey, iv))
            {
                using (ICryptoTransform iCryptoTransform = rijndaelManaged.CreateDecryptor())
                {
                    //转换hex格式数据为byte数组
                    byte[] buffer = new byte[value.Length / 2];
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (byte)Convert.ToInt32(value.Substring(i * 2, 2), 16);
                    }
                    buffer = iCryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer);
                }
            }
        }
        #endregion
    }
}
