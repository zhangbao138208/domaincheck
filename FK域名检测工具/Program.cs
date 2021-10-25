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
           
            
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var loginForm = new Login();
                if (DialogResult.OK == loginForm.ShowDialog())
                {
                    Application.Run(new Main(loginForm.UserName, loginForm.CompanyName, loginForm.CustomIndex));
                }
            }
            catch (Exception ex)
            {
 
                var strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now + "\r\n";
 
 
                var str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                    ex.GetType().Name, ex.Message, ex.StackTrace);
 
                
                LogHelper.Error(str);
               
            }
        }
    }
}
