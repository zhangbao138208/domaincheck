using System;
using System.Windows.Forms;

namespace FK域名检测工具
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var loginForm = new Login();
            if (DialogResult.OK == loginForm.ShowDialog())
            {
                Application.Run(new Main(loginForm.UserName, loginForm.CompanyName, loginForm.CustomIndex));
            }
        }
    }
}
