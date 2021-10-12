using System;
using System.Drawing;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public partial class Login : Form
    {
        public string UserName { get; private set; }
        public new string CompanyName { get; private set; }
        public int CustomIndex { get; private set; }
        public Login()
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
            LoadAccountInfo();
        }

        // placeholder效果
        private void textBox_password_Enter(object sender, EventArgs e)
        {
            if (textBox_password.Text != "请输入密码") return;
            textBox_password.ForeColor = Color.Black;
            textBox_password.UseSystemPasswordChar = true;
            textBox_password.Text = "";
        }

        private void textBox_password_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_password.Text)) return;
            textBox_password.ForeColor = Color.LightGray;
            textBox_password.UseSystemPasswordChar = false;
            textBox_password.Text = "请输入密码";
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
            RemoteLogin();
        }


        // 登录成功
        private void LoginSuccess(string userName, string product, int customIndex) {
            SaveAccountInfo(userName);
            this.UserName = userName;
            this.CompanyName = product;
            this.CustomIndex = customIndex;
            button_login.Enabled = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 登录失败
        private void LoginFailed() {
            label_errorInfo.Visible = true;
            textBox_password.Text = "";
            button_login.Enabled = true;
        }

        // 保存账密
        private static void SaveAccountInfo(string account) {
            IniConfigMgr.IniInstance.WriteConfig("账号", account, false);
        }

        // 从配置文件加载账号
        private void LoadAccountInfo() {
            var account = IniConfigMgr.IniInstance.LoadConfig("账号", false);
            if (string.IsNullOrEmpty(account)) {
                return;
            }

            textBox_account.Text = account;
        }

        // 尝试登录
        private void RemoteLogin() {
            var account = textBox_password.Text;
            var password = textBox_account.Text;

            var request = new Request();
            var loginRequest = new LoginRequest { 
                UserName = textBox_account.Text, 
                Password = AesHelper.AesEncrypt(textBox_password.Text, AesHelper.AES_KEY, AesHelper.AES_IV),
                Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV),
                IsManager = "0" 
            };
            //var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
            var apiPath = Api.Login;
            var result = request.Execute<LoginResponse>(apiPath, loginRequest.ToJson(), "POST");
            if (result is string s)
            {
                MessageBox.Show(s);
                LoginFailed();
            }
            else
            {
                var loginResponse = (LoginResponse)result;
                if (string.IsNullOrEmpty(loginResponse.Error))
                {
                    LoginSuccess(loginResponse.Username, loginResponse.Product, loginResponse.CustomIndex);
                }
                else
                {
                    MessageBox.Show(loginResponse.Error);
                    LoginFailed();
                }
            }
        }


        // 测试用
        private void TestLogin() {
            if (string.CompareOrdinal(textBox_password.Text, textBox_account.Text) == 0)
            {
                LoginSuccess(textBox_account.Text, "A01", 0);
            }
            else {
                LoginFailed();
            }
        }
    }
}
