using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FK域名检测工具管理器
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login loginForm = new Login();
            if (DialogResult.OK == loginForm.ShowDialog())
            {
                Application.Run(new Main(loginForm.UserName, loginForm.CompanyName));
            }
        }
    }
}
