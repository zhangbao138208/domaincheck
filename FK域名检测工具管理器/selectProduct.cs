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
    public partial class selectProduct : Form
    {
        public string productList;
        public selectProduct()
        {
            InitializeComponent();

            productList = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                productList += "A01";
            }
            if (checkBox2.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "A02";
            }
            if (checkBox3.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "A03";
            }
            if (checkBox4.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "A04";
            }
            if (checkBox5.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "A05";
            }
            if (checkBox6.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "A06";
            }
            if (checkBox7.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "B01";
            }
            if (checkBox8.Checked)
            {
                if (!string.IsNullOrEmpty(productList))
                {
                    productList += ",";
                }
                productList += "B02";
            }

            // NEW ADD
            this.DialogResult = DialogResult.OK;
            this.Close();
            // END OF NEW ADD
        }
    }
}
