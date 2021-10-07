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
            login loginForm = new login();
            if (DialogResult.OK == loginForm.ShowDialog())
            {
                Application.Run(new main(loginForm.userName, loginForm.compantName));
            }
        }
    }
}
