using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FK域名检测工具管理器
{
    public static class PictureFileUtility
    {
        public static bool SavePicture(byte [] pictureData, string filePath) {
            try
            {
                Bitmap b = CommonFunc.ByteToImage(pictureData);
                b.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
