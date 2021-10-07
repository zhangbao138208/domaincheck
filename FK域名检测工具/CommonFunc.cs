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
        public static string GetIPAddress2()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private static string GetIpData(string token, string ip = null, string datatype = "txt")
        {
            try
            {
                var url = "";
                url = string.IsNullOrEmpty(ip) ? $"https://api.ip138.com/ip/?datatype={datatype}&token={token}" : $"https://api.ip138.com/ip/?ip={ip}&datatype={datatype}&token={token}";
                using (var client = new WebClient())
                {
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
            try
            {
                var ip = "";
                if (isMustCertain)
                {
                    var data = GetIpData("415dcc20101f2dc52506d14c17c4aa6c");
                    if (!string.IsNullOrEmpty(data))
                    {
                        data = data.Replace(" ", "_");
                        data = data.Replace("\t", "_");
                        data = data.Replace("\n", "_");
                        System.Diagnostics.Debug.WriteLine("IP数据=" + data);
                        return data;
                    }

                    //var t0_html = HttpGetPageHtml("https://www.ip.cn", "utf-8");
                    //var t1_html = HttpGetPageHtml("http://www.ip138.com/ips138.asp", "gbk");
                    var t2_html = HttpGetPageHtml("http://www.net.cn/static/customercare/yourip.asp", "gbk");
                    //var t0_ip = GetIPFromHtml(t0_html);
                    //var t1_ip = GetIPFromHtml(t1_html);
                    var t2_ip = GetIPFromHtml(t2_html);

                    ip = t2_ip;
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = GetUncertainIPAddress();
                    }

                    if (IsIPAddress(ip))
                    {
                        string data2 = GetIpData("415dcc20101f2dc52506d14c17c4aa6c", ip);
                        if (string.IsNullOrEmpty(data2))
                        {
                            return ip;
                        }
                        else
                        {
                            data2 = data2.Replace(" ", "_");
                            data2 = data2.Replace("\t", "_");
                            data2 = data2.Replace("\n", "_");
                            System.Diagnostics.Debug.WriteLine("IP数据=" + data2);
                            return data2;
                        }
                    }
                }
                else
                {
                    ip = GetUncertainIPAddress();

                    if (IsIPAddress(ip))
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

        public static string HttpGetPageHtml(string url, string encoding)
        {
            string pageHtml = string.Empty;
            try
            {
                using (WebClient MyWebClient = new WebClient())
                {
                    Encoding encode = Encoding.GetEncoding(encoding);
                    MyWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.84 Safari/537.36");
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                    Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
                    pageHtml = encode.GetString(pageData);
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
        public static string GetIPFromHtml(String pageHtml)
        {
            //验证ipv4地址
            string reg = @"(?:(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))\.){3}(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))";
            string ip = "";
            Match m = Regex.Match(pageHtml, reg);
            if (m.Success)
            {
                ip = m.Value;
            }
            return ip;
        }

        /// <summary>  
        /// 获取CpuID  
        /// </summary>  
        /// <returns>CpuID</returns>  
        public static string GetCpuID()
        {
            try
            {
                string strCpuID = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return strCpuID;
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetMAC() {
            string s1 = GetMacAddress1();
            if (! string.IsNullOrEmpty(s1)) {
                return s1;
            }
            string s2 = GetMacAddress2();
            if (!string.IsNullOrEmpty(s2))
            {
                return s2;
            }
            return "00-00-00-00-00-00";
        }

        public static string GetMacAddress2() {
            try
            {
                const int MIN_MAC_ADDR_LENGTH = 12;
                long maxSpeed = -1;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    string tmpMac = nic.GetPhysicalAddress().ToString();
                    if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tmpMac) && tmpMac.Length >= MIN_MAC_ADDR_LENGTH)
                    {
                        return BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes());
                    }
                }
            }
            catch (Exception) { return ""; }
            return "";
        }


        public static string GetMacAddress1()
        {
            try
            {
                 string madAddr = "";
                 ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                 ManagementObjectCollection moc2 = mc.GetInstances();
                 foreach (ManagementObject mo in moc2)
                 {
                     if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                     {
                         madAddr = mo["MacAddress"].ToString();
                         madAddr = madAddr.Replace(':', '-');
                     }
                     mo.Dispose();
                 }
                 return madAddr;
            }
            catch
            {
                return "";
            }
        }


        public static string GetIP138Address() {
            try
            {
                
                string strUrl = "https://ipchaxun.com/";
                Uri uri = new Uri(strUrl);
                WebRequest wr = WebRequest.Create(uri);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();
                string ip = Regex.Match(all, @"(\d+\.\d+\.\d+\.\d+)").Groups[0].Value.ToString();
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
        public static bool IsIPAddress(string str1)
        {
            IPAddress ip;
            if (IPAddress.TryParse(str1, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetUncertainIPAddress() {
            try
            {
                //获取主机名
                string HostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string FindString(string url, string checkstring, int timeout) {
            try
            {
                LogHelper.Debug("FindString start");
                string htmlStr = "";
                if (!String.IsNullOrEmpty(url))
                {
                    Uri httpURL = new Uri(url);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httpURL);        //实例化WebRequest对象
                    request.Method = "GET";
                    request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";
                    request.CookieContainer = new CookieContainer();
                    request.Timeout = timeout;
                    WebResponse response = request.GetResponse();           //创建WebResponse对象
                    Stream datastream = response.GetResponseStream();       //创建流对象
                    Encoding ec = Encoding.UTF8;                         //可以指定网页编码方式
                    StreamReader reader = new StreamReader(datastream, ec);
                    LogHelper.Debug("FindString ReadToEnd");
                    htmlStr = reader.ReadToEnd();                           //读取数据
                    reader.Close();
                    datastream.Close();
                    response.Close();
                    LogHelper.Debug("FindString response.Close()");
                }
                bool result = (htmlStr.IndexOf(checkstring) != -1);
                //if (!result)
                //{
                //    MessageBox.Show(htmlStr, checkstring);
                //}
                if (result)
                {
                    LogHelper.Debug("FindString 成功");
                    return "成功";
                }
                else {
                    LogHelper.Debug("FindString 验证失败");
                    return "验证失败";
                }
            }
            catch (WebException ex)
            {
                LogHelper.Error("FindString WebException",ex);
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        LogHelper.Debug("FindString WebException 404失败");
                        return "404失败";
                    }
                    else if (resp.StatusCode == HttpStatusCode.Forbidden)
                    {
                        LogHelper.Debug("FindString WebException 403失败");
                        return "403失败";
                    }
                    LogHelper.Debug("FindString WebException 未知失败");
                    return "验证失败";
                }
                else
                {
                    LogHelper.Debug("FindString WebException 验证失败");
                    return "验证失败";
                }
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
            ImageConverter converter = new ImageConverter();
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
            if (string.Compare(fake, "BTT") == 0) {
                return "A01";
            } else if (string.Compare(fake, "AA") == 0)
            {
                return "A02";
            }
            else if (string.Compare(fake, "BY") == 0)
            {
                return "A03";
            }
            else if (string.Compare(fake, "ZZ") == 0)
            {
                return "A04";
            }
            else if (string.Compare(fake, "CC") == 0)
            {
                return "A05";
            }
            else if (string.Compare(fake, "KK") == 0)
            {
                return "A06";
            }
            else if (string.Compare(fake, "BB") == 0)
            {
                return "B01";
            }
            else if (string.Compare(fake, "YM") == 0)
            {
                return "C01";
            }
            return "Unknown";
        }
    }
}
