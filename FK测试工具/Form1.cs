using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FK测试工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            string result = "";
            string MAC1 = CommonFunc.GetMacAddress1();
            string MAC2 = CommonFunc.GetMacAddress2();
            string MACFinal = CommonFunc.GetMAC();

            result = String.Format(@"GetMacAddress1 = {0}
GetMacAddress2 = {1}
GetMAC = {2}",
                MAC1, MAC2, MACFinal);

            this.richTextBox_log.Text = result;
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(richTextBox_log.Text);
        }
    }
}
