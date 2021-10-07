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
    public partial class createDomain : Form
    {
        private DomainInfo ds;
        private bool isModify = false;
        private string creator;
        private List<string> checkPointList;
        public createDomain(DomainInfo ds, List<string> checkPointList, string creator, bool isModify)
        {
            InitializeComponent();

            this.ds = ds;
            this.checkPointList = checkPointList;
            this.isModify = isModify;
            this.creator = creator;
        }

        private void createDomain_Load(object sender, EventArgs e)
        {
            if (this.isModify) {
                this.label_title.Text = "修改域名";
                this.button_loadDomainFile.Visible = false;
            }
            else{
                this.label_title.Text = "创建域名";
                this.button_loadDomainFile.Visible = true;
            }

            if (string.Compare(this.ds.Product, "全部") == 0)
            {
                this.comboBox_company.Enabled = true;
                this.comboBox_company.SelectedIndex = 0;
            }
            else
            {
                this.comboBox_company.Enabled = false;
                for (int i = 0; i < this.comboBox_company.Items.Count; i++)
                {
                    string value = this.comboBox_company.GetItemText(this.comboBox_company.Items[i]);
                    if (string.Compare(value, this.ds.Product) == 0)
                    {
                        this.comboBox_company.SelectedIndex = i;
                        break;
                    }
                }
            }

            string product = this.comboBox_company.Text;

            this.comboBox_CPName.Items.Clear();
            for (int i = 0; i < this.checkPointList.Count; i++)
            {
                if (this.checkPointList[i].StartsWith(product))
                {
                    this.comboBox_CPName.Items.Add(this.checkPointList[i]);
                }
            }
            if (string.IsNullOrEmpty(this.ds.CheckpointIndex.ToString()))
            {
                if (this.comboBox_CPName.Items.Count > 0)
                {
                    this.comboBox_CPName.SelectedIndex = 0;
                }
            }
            else
            {
                this.comboBox_CPName.SelectedIndex = 0;
                for (int i = 0; i < this.comboBox_CPName.Items.Count; i++)
                {
                    string value = this.comboBox_CPName.GetItemText(this.comboBox_CPName.Items[i]);
                    string oldValue = this.ds.Product + "_" +  this.ds.CheckpointIndex.ToString();
                    if (string.Compare(value, oldValue) == 0)
                    {
                        this.comboBox_CPName.SelectedIndex = i;
                        break;
                    }
                }
            }


            this.richTextBox_domain.Text = "";
            if (!string.IsNullOrEmpty(this.ds.Domain)) {
                this.richTextBox_domain.Text = this.ds.Domain;
            }
 

            this.richTextBox_Remark.Text = "";
            if (!string.IsNullOrEmpty(this.ds.Comment)) {
                this.richTextBox_Remark.Text = this.ds.Comment;
            }
        }

        // 创建域名
        private void button_create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.richTextBox_domain.Text)) {
                MessageBox.Show("错误：域名禁止为空.");
                return;
            }
            if (string.IsNullOrEmpty(this.comboBox_CPName.Text)) {
                MessageBox.Show("错误：监测点禁止为空.");
                return;
            }
            if (this.isModify)
            {
                if (this.richTextBox_domain.Text.Contains(';')) {
                    MessageBox.Show("错误：修改域名时禁止添加多域名，如需要创建多域名，请使用创建.");
                    return;
                }
                // 修改
                var request = new Request();
                int checkpointIndex = Int32.Parse(comboBox_CPName.Text.Split('_')[1]);
                var updateDomainRequest = new UpdateDomainRequest
                {
                    DomainIndex = this.ds.DomainIndex,
                    Domain = this.richTextBox_domain.Text,
                    CheckpointIndex = checkpointIndex,
                    Product = this.comboBox_company.Text,
                    Creator = this.creator,
                    Comment = this.richTextBox_Remark.Text
                };
                var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
                var apiPath = "http://" + ip + ":80/v1/UpdateDomain";
                var result = request.Execute<UpdateDomainResponse>(apiPath, updateDomainRequest.ToJson(), "POST");
                if (result is string)
                {
                    MessageBox.Show("错误：" + result);
                }
            }
            else {
                // 创建
                var request = new Request();
                int checkpointIndex = Int32.Parse(comboBox_CPName.Text.Split('_')[1]);
                var createDomainRequest = new CreateDomainRequest
                {
                    Domain = this.richTextBox_domain.Text,
                    CheckpointIndex = checkpointIndex,
                    Product = this.comboBox_company.Text,
                    Creator = this.creator,
                    Comment = this.richTextBox_Remark.Text
                };
                var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
                var apiPath = "http://" + ip + ":80/v1/CreateDomain";
                var result = request.Execute<CreateDomainResponse>(apiPath, createDomainRequest.ToJson(), "POST");
                if (result is string)
                {
                    MessageBox.Show("错误：" + result);
                }
                else{
                    
                    CreateDomainResponse cdr = (CreateDomainResponse)result;
                    if (!string.IsNullOrEmpty(cdr.Error)) {
                        MessageBox.Show("错误：" + cdr.Error);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 关闭
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_loadDomainFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;//默认是1，则默认显示的文件类型为*.txt；如果设置为2，则默认显示的文件类型是*.*

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string text = File.ReadAllText(openFileDialog1.FileName);
                richTextBox_domain.Text = text;
            }
        }

        private void comboBox_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product = this.comboBox_company.Text;

            this.comboBox_CPName.Items.Clear();
            for (int i = 0; i < this.checkPointList.Count; i++)
            {
                if (this.checkPointList[i].StartsWith(product))
                {
                    this.comboBox_CPName.Items.Add(this.checkPointList[i]);
                }
            }
            if (this.comboBox_CPName.Items.Count > 0) {
                this.comboBox_CPName.SelectedIndex = 0;
            }
        }

        private void comboBox_CPName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
