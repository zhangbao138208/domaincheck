using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FK域名检测工具
{
    internal static class Program
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
                Application.Run(new Main(loginForm.userName, loginForm.compantName, loginForm.customIndex));
            }
        }
    }
}
