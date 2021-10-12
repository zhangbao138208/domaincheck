using System;
using System.Drawing;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace FK域名检测工具管理器
{
    public static class CommonFunc
    {
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {  // 总是接受  
            return true;
        }
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            return (byte[])obj;
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            try
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(blob))
                {
                    bmp = new Bitmap(ms);
                    return bmp;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
