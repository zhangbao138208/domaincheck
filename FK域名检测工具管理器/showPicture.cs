using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FK域名检测工具管理器
{
    public partial class showPicture : Form
    {
        private byte[] imageData;
        public showPicture(byte[] imageData)
        {
            InitializeComponent();

            this.imageData = imageData;
        }


        private void button_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void showPicture_Load(object sender, EventArgs e)
        {
            /*
            Bitmap b = CommonFunc.ByteToImage(this.imageData);
            b.Save(DateTime.Now.Year.ToString()
+ DateTime.Now.Month.ToString()
+ DateTime.Now.Day.ToString()
+ DateTime.Now.Hour.ToString()
+ DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()
+ ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            */
            // 显示图片
            Image b = CommonFunc.ByteToImage(this.imageData);
            if (b != null)
            {
                pictureBox1.Image = b;
            }
        }
    }
}
