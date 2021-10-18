using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FK域名检测工具管理器
{
    public partial class createCP : Form
    {
        private CPInfo cps;
        private bool isModify = false;
        private string creator;
        public createCP(CPInfo cps, string creator, bool isModify)
        {
            InitializeComponent();

            this.cps = cps;
            this.isModify = isModify;
            this.creator = creator;
        }

        private void createCP_Load(object sender, EventArgs e)
        {
            if (this.isModify)
            {
                this.label_title.Text = "修改监测点";
                this.comboBox_company.Enabled = false;
            }
            else
            {
                this.label_title.Text = "创建监测点";
                this.comboBox_company.Enabled = true;
            }

            if (string.Compare(this.cps.Product, "全部") == 0)
            {
                this.comboBox_company.Enabled = true;
                this.comboBox_company.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < this.comboBox_company.Items.Count; i++)
                {
                    string value = this.comboBox_company.GetItemText(this.comboBox_company.Items[i]);
                    if (string.Compare(value, this.cps.Product) == 0)
                    {
                        this.comboBox_company.SelectedIndex = i;
                        break;
                    }
                }
            }

            this.textBox_checkPath.Text = "";
            if (!string.IsNullOrEmpty(this.cps.CheckPath))
            {
                this.textBox_checkPath.Text = this.cps.CheckPath;
            }

            this.textBox_checkString.Text = "";
            if (!string.IsNullOrEmpty(this.cps.CheckString))
            {
                this.textBox_checkString.Text = this.cps.CheckString;
            }
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox_checkString.Text))
            {
                MessageBox.Show("错误：字符串 禁止为空.");
                return;
            }
            if (string.IsNullOrEmpty(this.textBox_checkPath.Text))
            {
                MessageBox.Show("错误：监测路径 禁止为空.");
                return;
            }

            if (this.isModify)
            {
                // 修改
                var request = new Request();
                var updateCPRequest = new UpdateCPRequest
                {
                    CheckpointIndex = this.cps.CheckpointIndex,
                    CheckSting = textBox_checkString.Text,
                    CheckPath = textBox_checkPath.Text,
                    Product = this.comboBox_company.Text,
                    Creator = this.creator
                };
                var apiPath = Api.UpdateCheckpoint;
                var result = request.Execute<UpdateCPResponse>(apiPath, updateCPRequest.ToJson(), "POST");
                if (result is string)
                {
                    MessageBox.Show("错误：" + result);
                }
            }
            else
            {
                // 创建
                var request = new Request();
                var createCPRequest = new CreateCPRequest
                {
                    CheckSting = textBox_checkString.Text,
                    CheckPath = textBox_checkPath.Text,
                    Product = this.comboBox_company.Text,
                    Creator = this.creator
                };
                var apiPath = Api.CreateCheckpoint;
                var result = request.Execute<CreateCPResponse>(apiPath, createCPRequest.ToJson(), "POST");
                if (result is string)
                {
                    MessageBox.Show("错误：" + result);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /*
        private void button_updateDomain_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;//默认是1，则默认显示的文件类型为*.txt；如果设置为2，则默认显示的文件类型是*.*

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string text = File.ReadAllText(openFileDialog1.FileName);
                richTextBox_domains.Text = text;
            }
        }
        */
    }
}
