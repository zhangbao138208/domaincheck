using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeoutException = System.TimeoutException;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            // LogHelper.Info("info");
            // LogHelper.Error("err",null);
            //int ret = 1;
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(1000*6);
            //     ret = 2;

            //}).Wait(5*1000);
            //LogHelper.Info(ret.ToString());
            var str = string.Empty;
            for (int i = 0; i < 3000; i++)
            {
                str += $"http://www.gooogle{i.ToString()}.com;";
            }
            str += "http://www.gooogle.com";
            Console.WriteLine(str);
            Console.ReadLine();
        }

        public static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }
    }

}
