using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FK域名检测工具管理器
{
    public static class CommonFunc
    {
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
