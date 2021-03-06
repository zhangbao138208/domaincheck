using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public partial class main : Form
    {
        public static CancellationTokenSource cs;

        private delegate void setRichTexBox(string s, Color color);
        private Thread g_MainThread;

        private string userName = "";
        private string companyName = "";
        private int customIndex = 0;
        public main(string userName, string companyName, int customIndex)
        {
            InitializeComponent();

            this.userName = userName;
            this.companyName = companyName;
            this.customIndex = customIndex;

            //HttpContext context = System.Web.HttpContext.Current;
            //HttpRuntime.Cache.Insert("context", context);
        }

        private void main_Load(object sender, EventArgs e)
        {
            this.label_company.Text = this.companyName;
            this.label_account.Text = this.userName;
            this.label_mac.Text = CommonFunc.GetMAC();

            this.richTextBox_updatelog.Text = @"
服务器版本查看接口为  域名/health (例如打开浏览器，输入 http://goapi.xxoo.com/health)

2021/9/24  版本0.4.3 （服务器1.0.6）
    1：修复未同步403,404失败的BUG
    2：修复未同步城市信息到服务器的BUG
2021/9/24  版本0.4.2 （服务器1.0.6）
    1：即使监测成功，也获取当前城市信息
2021/9/22  版本0.4.1 （服务器1.0.5）
    1：根据用户登录账号自动分配所属产品域名，去除手选项
    2：优化CURL检查结果判断，提高其准确率
2021/9/16: 版本0.4.0 （服務器1.0.5）
    1：更改ping為curl方案
    2：增加403失败，404失败的错误分类
    3：全部IP为真实外网IP
    4：增加IP对地址的解析
2021/9/3  版本0.3.8 （服务器1.0.4）
    1：追加客户端动态域名分配功能
2021/8/27 版本0.3.7
    1：ping失败时，追加对百度进行ping复检，以减少因网络波动引发的错误ping判定
    2：追加冗余域名填充，当前为1:1混比填充
2021/8/25 版本0.3.6
    1：加壳防止反编译
    2：增加静态冗余数据和内存混淆功能
2021/8/23 版本0.3.5
    1：修复了截图时会占时过短的问题，延长至60S
2021/8/23 版本0.3.4
    1：修复了截图时会占时过长的问题
2021/8/22 版本0.3.3
    1：修复MAC地址获取不到的问题
    2：修复域名解析偶尔失败的BUG
2021/8/21 版本0.3.2
    1：增加客户端安全方案1期
2021/8/16 版本0.3.1
    1：增加了客户端MAC地址展示，以便配置到服务器进行客户端的MAC地址限制
    2：更新各产品名
2021/8/6 版本0.3.0
    1：解决A05部分网站访问时强制要求cookie和UA导致访问错误的问题。
2021/8/1 版本0.2.9
    1：修复严重BUG，向服务器请求域名失败会引发退出线程。
2021/8/1 版本0.2.8
    1：解决个别网站始终加载，从不触发complete事件，导致长期【卡死】问题。
2021/7/31 版本0.2.7
    1：使用更高效的CancellationTokenSource的来避免线程死锁。
2021/7/31 版本0.2.6
    1：日志根据产品分开查看功能。
    2：管理器增加客户端活动列表功能。
2021/7/31 版本0.2.5
    1：增加网络最大链接，增加大量资源释放机制，以防止卡死。
2021/7/30 版本0.2.4
    1：尝试禁止JS运行产生新的网页和跳转连接。
2021/7/30 版本0.2.3
    1：增加了保护机制，即使【数据上报失败】也会继续下一步操作
2021/7/29 版本0.2.2
    1：增加部分安全检测功能
2021/7/28 版本0.2.1
    1：增加公网IP检查功能
2021/7/23 版本0.2.0
    1：功能完整版
2021/7/16 版本0.1.5
    1：修复了除截图外的全部BUG
2021/7/15 版本0.1.4
    1：修改【间隔】为【超时设置】
    2：隐藏用户所属产品
    3：增加【全选/取消全选】按钮便捷功能
    4：修复了【日志管理数据分页显示错误】BUG
2021/7/14 版本0.1.3
    1：修改了tb_logs记录的creator错误问题，0.1.1版本的修复不完整
2021/7/14 版本0.1.2
    1：修改了检查网页使用的编码格式为UTF8，解决了YYP网站验证字符串失败问题
2021/7/14 版本0.1.1
    1: 登录时同步了用户名和所属产品信息
    2: 修改了tb_logs记录的creator错误问题
2021/7/13 版本0.1.0
    1: 第一次发布给测试
            ";
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 更变间隔时间
        private void trackBar_elspTime_Scroll(object sender, EventArgs e)
        {
            label_elspTime.Text = trackBar_elspTime.Value.ToString();
        }

        // 开始按钮
        private void button_start_Click(object sender, EventArgs e)
        {
            if (g_MainThread != null) {
                return;
            }
            addNoticeLog("任务开始");
            cs = new CancellationTokenSource();
            g_MainThread = new Thread(ThreadMain)
            {
                //IsBackground = true
            };
            object [] param = new object[2] { trackBar_elspTime.Value * 1000, this.userName };
            // param.Add(trackBar_elspTime.Value * 1000);
            // param.Add(this.userName);
            g_MainThread.SetApartmentState(ApartmentState.STA);
            g_MainThread.Start(param);
        }

        private void ThreadMain(Object param) {
            List<DomainCheckCondition> needCheckedDomainList = new List<DomainCheckCondition>();
            int checkedDomainCount = 0;
            int checkFailedDomainCount = 0;
            object[] paramList = (object[])param;
            int timeout = (int)paramList[0];
            string username = (string)paramList[1];
            for (; ;)
            {
                Thread.Sleep(100);

                if (cs.Token.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    if (needCheckedDomainList.Count <= 0)
                    {
                        if (!requestDomain(needCheckedDomainList))
                        {
                            // 请求失败
                            continue;
                        }
                        else
                        {
                            // 请求成功
                        }
                    }
                    else
                    {
                        checkDomain(needCheckedDomainList, ref checkedDomainCount, ref checkFailedDomainCount, timeout, username);
                    }
                }
                catch (Exception) {
                    // addNoticeLog("出现异常，已捕获跳过..." + e.ToString());
                }
            }
        }

        // 暂停按钮
        private void button_pause_Click(object sender, EventArgs e)
        {
            if (g_MainThread == null) {
                addErrorLog("当前无工作线程，请先点击【开始】按钮");
                return; 
            }
            /*
            addNoticeLog("任务停止");
            g_MainThread.Abort();
            while (g_MainThread.ThreadState != ThreadState.Aborted) {
                Thread.Sleep(100);
            }
            g_MainThread = null;
            */
            cs.Cancel(false);
            cs.Token.Register(() =>
            {
                addNoticeLog("任务停止");
                g_MainThread.Abort();
                g_MainThread = null;
            });
        }


        // 跳转最后一行
        private void richTextBox_log_TextChanged(object sender, EventArgs e)
        {
            this.richTextBox_log.SelectionStart = this.richTextBox_log.TextLength;
            this.richTextBox_log.ScrollToCaret();
        }

        private void Set(string s, Color color)
        {
            this.richTextBox_log.SelectionColor = color;
            this.richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
        }
        private void addLog(string s) {
            if (this.richTextBox_log.InvokeRequired)
            {
                setRichTexBox fc = new setRichTexBox(Set);
                this.Invoke(fc, new object[] { s, Color.Gray });
            }
            else
            {
                this.richTextBox_log.SelectionColor = Color.Gray;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
            }
        }

        private void addNoticeLog(string s)
        {
            if (this.richTextBox_log.InvokeRequired)
            {
                setRichTexBox fc = new setRichTexBox(Set);
                this.Invoke(fc, new object[] { s, Color.Green });
            }
            else
            {
                this.richTextBox_log.SelectionColor = Color.Green;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
            }
        }

        private void addErrorLog(string s)
        {
            if (this.richTextBox_log.InvokeRequired)
            {
                setRichTexBox fc = new setRichTexBox(Set);
                this.Invoke(fc, new object[] { s, Color.Red });
            }
            else
            {
                this.richTextBox_log.SelectionColor = Color.Red;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
            }
        }

        // 请求5个域名
        private bool requestDomain(List<DomainCheckCondition> tobeCheckedDomainList) {
            List<string> strList = new List<string>();
            /*
            for (int i = 0; i < checkedListBox_product.Items.Count; i++)
            {
                if (checkedListBox_product.GetItemChecked(i))
                {
                    strList.Add( CommonFunc.FakeProductToRealProduct(checkedListBox_product.Items[i].ToString()));
                }
            }
            */
            if (string.Compare("全部", this.companyName) != 0)
            {
                strList.Add(this.companyName);
            }
            else {
                strList.Add("A01");
                strList.Add("A02");
                strList.Add("A03");
                strList.Add("A04");
                strList.Add("A05");
                strList.Add("A06");
                strList.Add("B01");
                strList.Add("C01");
            }

            if (strList.Count <= 0) {
                addErrorLog("请先选择至少一个部门.");
                return false;
            }
            addLog("向服务器请求新域名...");

            var request = new Request();
            var getDomainRequest = new GetDomainRequest { 
                Products = strList.ToArray(),
                CustomIndex = this.customIndex,
                MAC = AesHelper.AesEncrypt(CommonFunc.GetMAC(), AesHelper.AES_KEY, AesHelper.AES_IV)
            };
            var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
            var apiPath = "http://" + ip + ":80/v1/GetDomain";
            object re = request.Execute<GetDomainResponse>(apiPath,
                getDomainRequest.ToJson(), "POST");

            //addLog(getDomainRequest.ToJson());

            if (re is string)
            {
                addErrorLog("从服务器请求新域名失败...");
                return false;
            }
            else
            {
                var getDomainResponse = (GetDomainResponse)re;
                //var getDomainResponse = (GetDomainResponse)request.Execute<GetDomainResponse>(apiPath, 
                //    getDomainRequest.ToJson(), "POST");
                if (string.IsNullOrEmpty(getDomainResponse.Error))
                {
                    if (getDomainResponse.DomainCheckConditions == null)
                    {
                        addErrorLog("从服务器请求新域名失败...");
                        return false;
                    }
                    int domainCount = getDomainResponse.DomainCheckConditions.Count();
                    for (int i = 0; i < domainCount; i++)
                    {
                        tobeCheckedDomainList.Add(getDomainResponse.DomainCheckConditions[i]);

                        // 垃圾域名
                        tobeCheckedDomainList.Add(TrashDomains.GetRadomTrashDomainCheckCondition());
                    }
                    return true;
                }
                else
                {
                    addErrorLog("从服务器请求新域名失败... 错误信息：" + getDomainResponse.Error);
                    return false;
                }
            }
        }

        // 实际监测域名
        private void checkDomain(List<DomainCheckCondition> tobeCheckedDomainList, 
            ref int checkedDomainCount,ref int checkFailedDomainCount, int timeout, string username) 
        {
            if (tobeCheckedDomainList.Count() <= 0) {
                return;
            }
            checkedDomainCount++;

            addLog("正在监测第 " + checkedDomainCount.ToString() + " 个域名，已有 " + checkFailedDomainCount + " 个域名检查未通过.");
            DomainCheckCondition condition = tobeCheckedDomainList.First();
            tobeCheckedDomainList.RemoveAt(0);
            if (condition == null)
            {
                return;
            }

            // 数据加工
            string decryptedCheckDomain = AesHelper.AesDecrypt(condition.Domain, AesHelper.AES_KEY, AesHelper.AES_IV);

            //checkDomain = "https://w6609.net";

            string decryptedCheckPath = AesHelper.AesDecrypt(condition.CheckPath, AesHelper.AES_KEY, AesHelper.AES_IV);
            string decryptedCheckString = AesHelper.AesDecrypt(condition.CheckString, AesHelper.AES_KEY, AesHelper.AES_IV);
            if (!decryptedCheckDomain.StartsWith("http://") && !decryptedCheckDomain.StartsWith("https://"))
                decryptedCheckDomain = "https://" + decryptedCheckDomain;
            if (!decryptedCheckPath.StartsWith("/"))
                decryptedCheckPath = "/" + decryptedCheckPath;

            // STEP1：PING
            bool isCurlSuccess = false;
            bool isPingBaiduSuccess = false;
            try
            {
                /*
                Ping p1 = new Ping();
                Uri httpURL = new Uri(decryptedCheckDomain);
                PingReply reply = p1.Send(httpURL.Host, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    // 成功
                    isCurlSuccess = true;
                }
                else if (reply.Status == IPStatus.TimedOut)
                {
                    // 超时
                    isCurlSuccess = false;
                }
                else
                {
                    // 失败
                    isCurlSuccess = false;
                }
                */
                isCurlSuccess = Curl.Get(decryptedCheckDomain);

            }
            catch (Exception) {
                isCurlSuccess = false;
            }

            // 通过百度进行二次复检
            if (!isCurlSuccess) {
                try
                {
                    Ping p2 = new Ping();
                    Uri httpURL2 = new Uri("https://baidu.com");
                    PingReply reply2 = p2.Send(httpURL2.Host, timeout);
                    if (reply2.Status == IPStatus.Success)
                    {
                        // 成功
                        isPingBaiduSuccess = true;
                    }
                    else if (reply2.Status == IPStatus.TimedOut)
                    {
                        // 超时
                        isPingBaiduSuccess = false;
                    }
                    else
                    {
                        // 失败
                        isPingBaiduSuccess = false;
                    }
                }
                catch (Exception)
                {
                    isPingBaiduSuccess = false;
                }
            }

            if (!isCurlSuccess) {
                if (isPingBaiduSuccess) {
                    addErrorLog("域名CURL检查失败.");

                    if (!TrashDomains.IsTrashDomain(condition.Domain)) {
                        checkFailedDomainCount++;
                        var request = new Request();
                        var checkDomainResultRequest = new CheckDomainResultRequest
                        {
                            Domain = condition.Domain,
                            CheckIP = CommonFunc.GetIPAddressAndData(true),
                            Creator = username,
                            ClientID = CommonFunc.GetCpuID(),
                            Product = condition.Product,
                            Result = "CURL失败",
                            MAC = AesHelper.AesEncrypt(CommonFunc.GetMAC(), AesHelper.AES_KEY, AesHelper.AES_IV),
                            Printscreen = new byte[0]
                        };
                        var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
                        var apiPath = "http://" + ip + ":80/v1/SyncCheckResult";
                        var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                        if (result is string)
                        {
                            addErrorLog("数据上报失败:" + result);
                        }
                        return;
                    }
                }
                else
                {
                    addErrorLog("域名CURL失败和百度Ping失败，请检查网络.");
                    return;
                }
            }

            // STEP2: 检查匹配
            if (!string.IsNullOrEmpty(decryptedCheckPath) && !string.IsNullOrEmpty(decryptedCheckString))
            {
                string sResult = string.Empty;
                try
                {
                    sResult = CommonFunc.FindString(decryptedCheckDomain + decryptedCheckPath, decryptedCheckString, timeout);
                }
                catch (Exception)
                {
                    sResult = "验证失败";
                }

                if (string.Compare(sResult, "成功") != 0)
                {
                    addErrorLog("字符串验证失败.");

                    if (!TrashDomains.IsTrashDomain(condition.Domain))
                    {
                        checkFailedDomainCount++;
                        var request = new Request();
                        decryptedCheckDomain = AesHelper.AesDecrypt(condition.Domain, AesHelper.AES_KEY, AesHelper.AES_IV);
                        if (!decryptedCheckDomain.StartsWith("http://") && !decryptedCheckDomain.StartsWith("https://"))
                            decryptedCheckDomain = "https://" + decryptedCheckDomain;
                        byte[] b = printscreen(decryptedCheckDomain);
                        if (b == null)
                        {
                            b = new byte[0];
                        }
                        var checkDomainResultRequest = new CheckDomainResultRequest
                        {
                            Domain = condition.Domain,
                            CheckIP = CommonFunc.GetIPAddressAndData(true),
                            Creator = username,
                            ClientID = CommonFunc.GetCpuID(),
                            Product = condition.Product,
                            Result = sResult,
                            MAC = AesHelper.AesEncrypt(CommonFunc.GetMAC(), AesHelper.AES_KEY, AesHelper.AES_IV),
                            Printscreen = b
                        };
                        var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
                        var apiPath = "http://" + ip + ":80/v1/SyncCheckResult";
                        var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                        if (result is string)
                        {
                            addErrorLog("数据上报失败:" + result);
                        }
                        return;
                    }
                }
            }

            // STEP3: 全通过
            {
                if (!TrashDomains.IsTrashDomain(condition.Domain))
                {
                    var request = new Request();
                    var checkDomainResultRequest = new CheckDomainResultRequest
                    {
                        Domain = condition.Domain,
                        CheckIP = CommonFunc.GetIPAddressAndData(true),
                        Creator = username,
                        ClientID = CommonFunc.GetCpuID(),
                        Product = condition.Product,
                        Result = "成功",
                        MAC = AesHelper.AesEncrypt(CommonFunc.GetMAC(), AesHelper.AES_KEY, AesHelper.AES_IV),
                        Printscreen = new byte[0]
                    };
                    var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP", false);
                    var apiPath = "http://" + ip + ":80/v1/SyncCheckResult";
                    var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                    if (result is string)
                    {
                        addErrorLog("数据上报失败:" + result);
                    }
                }
            }
        }

        private byte[] printscreen(string url) {
            Bitmap bitmap = WebSiteThumbnail.GetWebSiteThumbnail(url, 800, 600, 400, 300);
            if (bitmap == null) {
                return null;
            }
            byte[] result = CommonFunc.ImageToByte(bitmap);
            return result;

            /*
            Bitmap b = CommonFunc.ByteToImage(result);
            b.Save(DateTime.Now.Year.ToString()
+ DateTime.Now.Month.ToString()
+ DateTime.Now.Day.ToString()
+ DateTime.Now.Hour.ToString()
+ DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()
+ ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            */
        }


        private void button_selectAllProduct_Click(object sender, EventArgs e)
        {
            if (string.Compare(button_selectAllProduct.Text, "全选") == 0) {
                for (int i = 0; i < this.checkedListBox_product.Items.Count; i++)
                {
                    this.checkedListBox_product.SetItemChecked(i, true);
                }
                this.button_selectAllProduct.Text = "取消全选";
            }
            else if (string.Compare(button_selectAllProduct.Text, "取消全选") == 0)
            {
                for (int i = 0; i < this.checkedListBox_product.Items.Count; i++)
                {
                    this.checkedListBox_product.SetItemChecked(i, false);
                }
                this.button_selectAllProduct.Text = "全选";
            }
        }
    }
}
