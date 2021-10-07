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
    public partial class login : Form
    {
        public string userName { get; set; }
        public string compantName { get; set; }
        public login()
        {
            InitializeComponent();
        }

        // 登录按钮
        private void button_login_Click(object sender, EventArgs e)
        {
            label_errorInfo.Visible = false;
            button_login.Enabled = false;

            // 登录
            // testLogin();
            // 向服务器申请登录
            loginToServer();
        }

        // 尝试登录
        private void loginToServer()
        {
            var request = new Request();
            var loginRequest = new LoginRequest { 
                UserName = textBox_account.Text, 
                Password = AesHelper.AesEncrypt(textBox_password.Text, AesHelper.AES_KEY, AesHelper.AES_IV),
                MAC = "",
                IsManager = "1" 
            };
            var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
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
                    loginSuccessed(loginResponse.Username, loginResponse.Product);
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
        private void testLogin()
        {
            if (String.Compare(textBox_password.Text, textBox_account.Text) == 0)
            {
                loginSuccessed(textBox_password.Text, "A01");
            }
            else
            {
                loginFailed();
            }
        }

        // 登录成功
        private void loginSuccessed(string userName, string company)
        {
            button_login.Enabled = true;
            this.userName = userName;
            this.compantName = company;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 登录失败
        private void loginFailed()
        {
            userName = "";
            compantName = "";
            label_errorInfo.Visible = true;
            textBox_password.Text = "";
            button_login.Enabled = true;
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
    }
}
