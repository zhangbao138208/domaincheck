using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public partial class login : Form
    {
        public string userName { get; set; }
        public string compantName { get; set; }
        public int customIndex { get; set; }
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            // placeholder效果
            textBox_password.ForeColor = Color.LightGray;
            textBox_password.UseSystemPasswordChar = false;
            textBox_password.Text = "请输入密码";

            label_MAC.Text = CommonFunc.GetMac();

            // 加载保存的账密
            loadAccountInfo();
        }

        // placeholder效果
        private void textBox_password_Enter(object sender, EventArgs e)
        {
            if (textBox_password.Text == "请输入密码")
            {
                textBox_password.ForeColor = Color.Black;
                textBox_password.UseSystemPasswordChar = true;
                textBox_password.Text = "";
            }
        }

        private void textBox_password_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_password.Text))
            {
                textBox_password.ForeColor = Color.LightGray;
                textBox_password.UseSystemPasswordChar = false;
                textBox_password.Text = "请输入密码";
            }
        }

        // 禁止空格
        private void textBox_account_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z')
                || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                if (label_errorInfo.Visible)
                {
                    label_errorInfo.Visible = false;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z')
                || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                if (label_errorInfo.Visible)
                {
                    label_errorInfo.Visible = false;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        // 登录按钮
        private void button_login_Click(object sender, EventArgs e)
        {
            label_errorInfo.Visible = false;
            button_login.Enabled = false;

            // 单机登录
            // testLogin();
            // 向服务器申请登录
            remoteLogin();
        }


        // 登录成功
        private void loginSuccessed(string userName, string product, int customIndex) {
            saveAccountInfo(userName);
            this.userName = userName;
            this.compantName = product;
            this.customIndex = customIndex;
            button_login.Enabled = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 登录失败
        private void loginFailed() {
            label_errorInfo.Visible = true;
            textBox_password.Text = "";
            button_login.Enabled = true;
        }

        // 保存账密
        private void saveAccountInfo(String account) {
            IniConfigMgr.IniInstance.WriteConfig("账号", account, false);
        }

        // 从配置文件加载账号
        private void loadAccountInfo() {
            string account = IniConfigMgr.IniInstance.LoadConfig("账号", false);
            if (String.IsNullOrEmpty(account)) {
                return;
            }

            textBox_account.Text = account;
        }

        // 尝试登录
        private void remoteLogin() {
            string account = textBox_password.Text;
            string password = textBox_account.Text;

            var request = new Request();
            var loginRequest = new LoginRequest { 
                UserName = textBox_account.Text, 
                Password = AesHelper.AesEncrypt(textBox_password.Text, AesHelper.AES_KEY, AesHelper.AES_IV),
                Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV),
                IsManager = "0" 
            };
            var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
            var apiPath = "http://" + ip + "/v1/Login";
            var result = request.Execute<LoginResponse>(apiPath, loginRequest.ToJson(), "POST");
            if (result is string)
            {
                MessageBox.Show((string)result);
                loginFailed();
            }
            else
            {
                var loginResponse = (LoginResponse)result;
                if (string.IsNullOrEmpty(loginResponse.Error))
                {
                    loginSuccessed(loginResponse.Username, loginResponse.Product, loginResponse.CustomIndex);
                }
                else
                {
                    MessageBox.Show(loginResponse.Error);
                    loginFailed();
                }
            }

            return;
        }


        // 测试用
        private void testLogin() {
            if (String.Compare(textBox_password.Text, textBox_account.Text) == 0)
            {
                loginSuccessed(textBox_account.Text, "A01", 0);
            }
            else {
                loginFailed();
            }
        }
    }
}
