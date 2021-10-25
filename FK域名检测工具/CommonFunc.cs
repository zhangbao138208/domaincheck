using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace FK域名检测工具
{
    public static class CommonFunc
    {
        public static string GetIpAddress2()
        {
            var context = System.Web.HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress)) return context.Request.ServerVariables["REMOTE_ADDR"];
            var addresses = ipAddress.Split(',');
            return addresses.Length != 0 ? addresses[0] : context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private static string GetIpData(string token, string ip = null, string datatype = "txt")
        {
            try
            {
                LogHelper.Debug($"GetIpData start");
                var url = string.IsNullOrEmpty(ip) ? $"https://api.ip138.com/ip/?datatype={datatype}&token={token}" : $"https://api.ip138.com/ip/?ip={ip}&datatype={datatype}&token={token}";
                using (var client = new WebClient())
                {
                    LogHelper.Debug($"GetIpData WebClient");
                    client.Encoding = Encoding.UTF8;
                    return client.DownloadString(url);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetIpAddressAndData(bool isMustCertain)
        {
            LogHelper.Debug($"GetIpAddressAndData start");
            try
            {
                string ip;
                if (isMustCertain)
                {
                    LogHelper.Debug($"GetIpAddressAndData isMustCertain");
                    var data = GetIpData("415dcc20101f2dc52506d14c17c4aa6c");
                    if (!string.IsNullOrEmpty(data))
                    {
                        LogHelper.Debug($"GetIpAddressAndData IsNullOrEmpty");
                        data = data.Replace(" ", "_");
                        data = data.Replace("\t", "_");
                        data = data.Replace("\n", "_");
                        System.Diagnostics.Debug.WriteLine("IP数据=" + data);
                        return data;
                    }

                    //var t0_html = HttpGetPageHtml("https://www.ip.cn", "utf-8");
                    //var t1_html = HttpGetPageHtml("http://www.ip138.com/ips138.asp", "gbk");
                    LogHelper.Debug($"GetIpAddressAndData t2Html");
                    var t2Html = HttpGetPageHtml("http://www.net.cn/static/customercare/yourip.asp", "gbk");
                    //var t0_ip = GetIPFromHtml(t0_html);
                    //var t1_ip = GetIPFromHtml(t1_html);
                    var t2Ip = GetIpFromHtml(t2Html);

                    ip = t2Ip;
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = GetUncertainIpAddress();
                    }
                    LogHelper.Debug($"IsIpAddress(ip) start");
                    if (IsIpAddress(ip))
                    {
                        var data2 = GetIpData("415dcc20101f2dc52506d14c17c4aa6c", ip);
                        if (string.IsNullOrEmpty(data2))
                        {
                            return ip;
                        }

                        data2 = data2.Replace(" ", "_");
                        data2 = data2.Replace("\t", "_");
                        data2 = data2.Replace("\n", "_");
                        System.Diagnostics.Debug.WriteLine("IP数据=" + data2);
                        return data2;
                    }
                }
                else
                {
                    ip = GetUncertainIpAddress();

                    if (IsIpAddress(ip))
                    {
                        return ip;
                    }
                }

            }
            catch (Exception) {
                return "";
            }
            return "";
        }

        private static string HttpGetPageHtml(string url, string encoding)
        {
            LogHelper.Debug($"HttpGetPageHtml start");
            string pageHtml;
            try
            {
                using (var myWebClient = new WebClient())
                {
                    LogHelper.Debug($"HttpGetPageHtml myWebClient");
                    var encode = Encoding.GetEncoding(encoding);
                    myWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.84 Safari/537.36");
                    myWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                    var pageData = myWebClient.DownloadData(url); //从指定网站下载数据
                    LogHelper.Debug($"HttpGetPageHtml GetString");
                    pageHtml = encode.GetString(pageData);
                    LogHelper.Debug($"HttpGetPageHtml GetString end");
                }
            }
            catch (Exception)
            {
                return "";
            }
            return pageHtml;
        }
        /// <summary>
        /// 从html中通过正则找到ip信息(只支持ipv4地址)
        /// </summary>
        /// <param name="pageHtml"></param>
        /// <returns></returns>
        private static string GetIpFromHtml(String pageHtml)
        {
            LogHelper.Debug($"GetIpFromHtml start");
            //验证ipv4地址
            const string reg = @"(?:(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))\.){3}(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))";
            var ip = "";
            var m = Regex.Match(pageHtml, reg);
            if (m.Success)
            {
                ip = m.Value;
            }
            LogHelper.Debug($"GetIpFromHtml end");
            return ip;
        }

        /// <summary>  
        /// 获取CpuID  
        /// </summary>  
        /// <returns>CpuID</returns>  
        public static string GetCpuId()
        {
            try
            {
                var strCpuId = string.Empty;
                var mc = new ManagementClass("Win32_Processor");
                var moc = mc.GetInstances();
                foreach (var o in moc)
                {
                    var mo = (ManagementObject) o;
                    strCpuId = mo.Properties["ProcessorId"].Value.ToString();
                }

                return strCpuId;
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetMac() {
            var s1 = GetMacAddress1();
            if (! string.IsNullOrEmpty(s1)) {
                return s1;
            }
            var s2 = GetMacAddress2();
            return !string.IsNullOrEmpty(s2) ? s2 : "00-00-00-00-00-00";
        }

        private static string GetMacAddress2() {
            try
            {
                const int minMacAdderLength = 12;
                const long maxSpeed = -1;
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var tmpMac = nic.GetPhysicalAddress().ToString();
                    if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tmpMac) && tmpMac.Length >= minMacAdderLength)
                    {
                        return BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes());
                    }
                }
            }
            catch (Exception) { return ""; }
            return "";
        }


        private static string GetMacAddress1()
        {
            try
            {
                 var madAdder = "";
                 var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                 var moc2 = mc.GetInstances();
                 foreach (var o in moc2)
                 {
                     var mo = (ManagementObject) o;
                     if (Convert.ToBoolean(mo["IPEnabled"]))
                     {
                         madAdder = mo["MacAddress"].ToString();
                         madAdder = madAdder.Replace(':', '-');
                     }
                     mo.Dispose();
                 }
                 return madAdder;
            }
            catch
            {
                return "";
            }
        }


        public static string GetIp138Address() {
            try
            {
                
                const string strUrl = "https://ipchaxun.com/";
                var uri = new Uri(strUrl);
                var wr = WebRequest.Create(uri);
                var s = wr.GetResponse().GetResponseStream();
                if (s == null) return "";
                var sr = new StreamReader(s, Encoding.Default);
                var all = sr.ReadToEnd();
                var ip = Regex.Match(all, @"(\d+\.\d+\.\d+\.\d+)").Groups[0].Value;
                return ip;

                /*
                string strUrl = "http://www.ip138.com/ip2city.asp";
                Uri uri = new Uri(strUrl);
                WebRequest webreq = WebRequest.Create(uri);
                Stream s = webreq .GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();
                int i = all.IndexOf("[") + 1;
                string tempip = all.Substring(i, 15);
                string ip = tempip.Replace("]", "").Replace(" ", "").Replace("<","");
                return ip;
                */
            }
            catch (Exception)
            {
                return "";
            }
        }

        //是否ip格式
        private static bool IsIpAddress(string str1)
        {
            return IPAddress.TryParse(str1, out _);
        }

        private static string GetUncertainIpAddress() {
            try
            {
                //获取主机名
                var hostName = Dns.GetHostName();
                var ipEntry = Dns.GetHostEntry(hostName);
                foreach (var t in ipEntry.AddressList)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (t.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return t.ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string FindString(string url, string checkString, int timeout) {
            try
            {
                LogHelper.Debug("FindString start");
                var htmlStr = "";
                if (!string.IsNullOrEmpty(url))
                {
                    var httpUrl = new Uri(url);
                    ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                    var request = (HttpWebRequest)WebRequest.Create(httpUrl);        //实例化WebRequest对象
                    request.Method = "GET";
                    request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";
                    request.CookieContainer = new CookieContainer();
                    request.Timeout = timeout;
                    var response = request.GetResponse();           //创建WebResponse对象
                    var dataStream = response.GetResponseStream();       //创建流对象
                    var ec = Encoding.UTF8;                         //可以指定网页编码方式
                    if (dataStream != null)
                    {
                        var reader = new StreamReader(dataStream, ec);
                        LogHelper.Debug("FindString ReadToEnd");
                        htmlStr = reader.ReadToEnd();                           //读取数据
                        reader.Close();
                    }

                    dataStream?.Close();
                    response.Close();
                    LogHelper.Debug("FindString response.Close()");
                }
                var result = (htmlStr.IndexOf(checkString, StringComparison.Ordinal) != -1);
                //if (!result)
                //{
                //    MessageBox.Show(htmlStr, checkstring);
                //}
                if (result)
                {
                    LogHelper.Debug("FindString 成功");
                    return "成功";
                }

                LogHelper.Debug("FindString 验证失败");
                return "验证失败";
            }
            catch (WebException ex)
            {
                LogHelper.Error("FindString WebException",ex);
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            LogHelper.Debug("FindString WebException 404失败");
                            return "404失败";
                        case HttpStatusCode.Forbidden:
                            LogHelper.Debug("FindString WebException 403失败");
                            return "403失败";
                        default:
                            LogHelper.Debug("FindString WebException 未知失败");
                            return "验证失败";
                    }
                }

                LogHelper.Debug("FindString WebException 验证失败");
                return "验证失败";
            }
            catch (Exception ex) {
                LogHelper.Error("FindString Exception", ex);
                return "验证失败";
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {  // 总是接受  
            return true;
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            try
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(blob))
                {
                    bmp = new Bitmap(ms);
                    return bmp;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string FakeProductToRealProduct(string fake) {
            if (string.CompareOrdinal(fake, "BTT") == 0) {
                return "A01";
            }

            if (string.CompareOrdinal(fake, "AA") == 0)
            {
                return "A02";
            }

            if (string.CompareOrdinal(fake, "BY") == 0)
            {
                return "A03";
            }

            if (string.CompareOrdinal(fake, "ZZ") == 0)
            {
                return "A04";
            }

            if (string.CompareOrdinal(fake, "CC") == 0)
            {
                return "A05";
            }

            if (string.CompareOrdinal(fake, "KK") == 0)
            {
                return "A06";
            }

            if (string.CompareOrdinal(fake, "BB") == 0)
            {
                return "B01";
            }

            return string.CompareOrdinal(fake, "YM") == 0 ? "C01" : "Unknown";
        }
    }
}
