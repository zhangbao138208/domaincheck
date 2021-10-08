using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FK域名检测工具
{
    public static class Curl
    {

        public static bool Get(string url)
        {
            var ret = false;
            Task.Factory.StartNew(() =>
            {
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
                        LogHelper.Debug("Curl Process start");
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
                        LogHelper.Debug("Curl WaitForExit");
                        p.WaitForExit();
                        p.Close();
                        LogHelper.Debug("Curl p.Close()");

                        Trace.WriteLine(url);
                        Trace.WriteLine(ss);

                        // 用windows的 curl.exe 测试就理解
                        if ((!ss.StartsWith("curl")) && (!string.IsNullOrEmpty(ss)))
                        {
                            LogHelper.Debug("Curl (!ss.StartsWith('curl')) && (!string.IsNullOrEmpty(ss))");
                            ret = true;
                        }
                        else
                        {
                            LogHelper.Debug("Curl return false");
                            ret = false;
                        }
                        
                    }
                }
                catch (Exception e){
                    LogHelper.Error("Curl Exception",e);
                    ret = false;
                }
                
            }).Wait(10*1000);
            return ret;
            
        }
    }
}
