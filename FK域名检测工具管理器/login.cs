using System;
using System.Windows.Forms;

namespace FK域名检测工具管理器
{
    public partial class Login : Form
    {
        public string UserName { get; private set; }
        public new string CompanyName { get; private set; }
        public Login()
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
            LoginToServer();
        }

        // 尝试登录
        private void LoginToServer()
        {
            var request = new Request();
            var loginRequest = new LoginRequest { 
                UserName = textBox_account.Text, 
                Password = AesHelper.AesEncrypt(textBox_password.Text, AesHelper.AES_KEY, AesHelper.AES_IV),
                MAC = "",
                IsManager = "1" 
            };
            var apiPath = Api.Login;
            var result = request.Execute<LoginResponse>(apiPath, loginRequest.ToJson(), "POST");
            if (result is string)
            {
                MessageBox.Show((string)result);
                LoginFailed();
            }
            else
            {
                var loginResponse = (LoginResponse)result;
                if (string.IsNullOrEmpty(loginResponse.Error))
                {
                    LoginSuccess(loginResponse.Username, loginResponse.Product);
                }
                else
                {
                    MessageBox.Show(loginResponse.Error);
                    LoginFailed();
                }
            }
        }


        // 测试用
        private void TestLogin()
        {
            if (string.CompareOrdinal(textBox_password.Text, textBox_account.Text) == 0)
            {
                LoginSuccess(textBox_password.Text, "A01");
            }
            else
            {
                LoginFailed();
            }
        }

        // 登录成功
        private void LoginSuccess(string userName, string company)
        {
            button_login.Enabled = true;
            this.UserName = userName;
            this.CompanyName = company;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 登录失败
        private void LoginFailed()
        {
            UserName = "";
            CompanyName = "";
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
