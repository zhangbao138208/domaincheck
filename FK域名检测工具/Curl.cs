using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FK域名检测工具
{
    public static class Curl
    {

        public static bool Get(string url) {
            try
            {
                //var rr = Guid.NewGuid().ToString();
                //string tmpFile = Path.GetTempPath() + "/tmp_" + rr + ".txt";
                LogHelper.Debug("Curl start");
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        FileName = "curl.exe",
                        Arguments = $" {url}",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    };
                    p.Start();
                    p.StandardInput.AutoFlush = true;

                    /*
                    int count = 0;
                    while (count < 2000)
                    {
                        Thread.Sleep(10);
                        if (File.Exists(tmpFile))
                        {
                            while (true)
                            {
                                try
                                {
                                    var ts = File.ReadAllText(tmpFile);
                                    break;
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(10);
                                }
                            }
                            break;
                        }
                        count++;
                    }
                    var ss = File.ReadAllText(tmpFile);
                    File.Delete(tmpFile);
                    */

                    //string ss = p.StandardOutput.ReadToEnd();
                    var output = new List<string>();

                    while (p.StandardOutput.Peek() > -1)
                    {
                        LogHelper.Debug("Curl p.StandardOutput.Peek() > -1");
                        output.Add(p.StandardOutput.ReadLine());
                    }

                    //while (p.StandardError.Peek() > -1)
                    //{
                    //    output.Add(p.StandardError.ReadLine());
                    //}
                    string ss = string.Join("", output.ToArray());

                    p.WaitForExit();
                    p.Close();
                    LogHelper.Debug("Curl p.Close()");

                    Trace.WriteLine(url);
                    Trace.WriteLine(ss);

                    // 用windows的 curl.exe 测试就理解
                    if ((!ss.StartsWith("curl")) && (!string.IsNullOrEmpty(ss)))
                    {
                        LogHelper.Debug("Curl (!ss.StartsWith('curl')) && (!string.IsNullOrEmpty(ss))");
                        return true;
                    }
                    LogHelper.Debug("Curl return false");
                    return false;
                }
            }
            catch (Exception e){
                LogHelper.Error("Curl Exception",e);
                return false;
            }
        }
    }
}
