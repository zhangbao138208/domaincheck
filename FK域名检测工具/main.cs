using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public partial class Main : Form
    {
        private static CancellationTokenSource _cs;

        private delegate void SetRichTexBox(string s, Color color);
        private Thread _gMainThread;

        private readonly string _userName;
        private readonly string _companyName;
        private readonly int _customIndex;
        private object _lock = new object();
        private int _timer;
        private bool _is_heartbeat;

       
        public Main(string userName, string companyName, int customIndex)
        {
            InitializeComponent();

            this._userName = userName;
            this._companyName = companyName;
            this._customIndex = customIndex;
            var heartbeat = IniConfigMgr.IniInstance.LoadConfig("Heartbeat");
            LogHelper.Debug($"Heartbeat is {heartbeat}");
            if (heartbeat != "true") return;
            _is_heartbeat = true;
            Control.CheckForIllegalCrossThreadCalls = false;


            //HttpContext context = System.Web.HttpContext.Current;
            //HttpRuntime.Cache.Insert("context", context);
        }

        private void main_Load(object sender, EventArgs e)
        {
            // 心跳检测
            if (_is_heartbeat)
            {
                Task.Factory.StartNew(() => {
                    while (true)
                    {
                        Thread.Sleep(5000);
                        if (_gMainThread != null)
                        {
                            AddNoticeLog($"心跳检测：{_gMainThread.IsAlive}");
                            if (!_gMainThread.IsAlive||_timer > 120)
                            {
                                _cs.Cancel(false);
                                _cs.Token.Register(() =>
                                {
                                    AddNoticeLog($"心跳重启:【IsAlive:{_gMainThread.IsAlive}】【_timer:{_timer}】");
                                    _gMainThread.Abort();
                                    _gMainThread = null;
                                    Action<object,EventArgs> ac = new Action<object, EventArgs>(button_start_Click);
                                    ac.Invoke(null,null);
                                });
                           
                            
                            }
                       
                        }
                    }
                });
            }
           this.label_company.Text = this._companyName;
            this.label_account.Text = this._userName;
            this.label_mac.Text = CommonFunc.GetMac();

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
        private void trackBar_time_scroll(object sender, EventArgs e)
        {
            label_elspTime.Text = trackBar_elspTime.Value.ToString();
        }

        // 开始按钮
        private void button_start_Click(object sender, EventArgs e)
        {
            if (_gMainThread != null) {
                return;
            }
            AddNoticeLog("任务开始");
            _cs = new CancellationTokenSource();
            _gMainThread = new Thread(ThreadMain)
            {
                //IsBackground = true
            };
            var param = new object[] { trackBar_elspTime.Value * 1000, _userName };
            _gMainThread.SetApartmentState(ApartmentState.STA);
            _gMainThread.Start(param);
        }

        private void ThreadMain(Object param) {
            List<DomainCheckCondition> needCheckedDomainList = new List<DomainCheckCondition>();
            int checkedDomainCount = 0;
            int checkFailedDomainCount = 0;
            object[] paramList = (object[])param;
            int timeout = (int)paramList[0];
            string username = (string)paramList[1];
            LogHelper.Debug("循环开始前");
            if (_is_heartbeat)
            {
                Task.Factory.StartNew(()=>
                {
                    Thread.Sleep(1000);
                    lock (_lock)
                    {
                        _timer++;
                    }
                
                });
            }
            for (; ;)
            {
                if (_is_heartbeat)
                {
                    lock (_lock)
                    {
                        _timer = 0;
                    }
                }
                Thread.Sleep(100);
                LogHelper.Debug("sleep 100ms");
                if (_cs.Token.IsCancellationRequested)
                {
                    LogHelper.Debug("IsCancellationRequested break");
                    break;
                }

                try
                {
                    if (needCheckedDomainList.Count <= 0)
                    {
                        LogHelper.Debug("needCheckedDomainList.Count <= 0");
                        LogHelper.Debug(!RequestDomain(needCheckedDomainList) ? "请求失败 continue" : "请求成功");
                    }
                    else
                    {
                        LogHelper.Debug("checkDomain");
                        CheckDomain(needCheckedDomainList, ref checkedDomainCount, ref checkFailedDomainCount, timeout, username);
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
            if (_gMainThread == null) {
                AddErrorLog("当前无工作线程，请先点击【开始】按钮");
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
            _cs.Cancel(false);
            _cs.Token.Register(() =>
            {
                AddNoticeLog("任务停止");
                _gMainThread.Abort();
                _gMainThread = null;
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
        private void AddLog(string s) {
            LogHelper.Debug(s);
            if (this.richTextBox_log.InvokeRequired)
            {
                var fc = new SetRichTexBox(Set);
                this.Invoke(fc, new object[] { s, Color.Gray });
            }
            else
            {
                this.richTextBox_log.SelectionColor = Color.Gray;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
                
            }
        }

        private void AddNoticeLog(string s)
        {
            LogHelper.Info(s);
            if (richTextBox_log.InvokeRequired)
            {
                var fc = new SetRichTexBox(Set);
                Invoke(fc, s, Color.Green);
            }
            else
            {
                richTextBox_log.SelectionColor = Color.Green;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
                
            }
        }

        private void AddErrorLog(string s)
        {
            LogHelper.Error(s);
            if (richTextBox_log.InvokeRequired)
            {
                var fc = new SetRichTexBox(Set);
                Invoke(fc, s, Color.Red);
            }
            else
            {
                richTextBox_log.SelectionColor = Color.Red;
                richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  -  ") + s + "\n");
               
            }
        }

        // 请求5个域名
        private bool RequestDomain(List<DomainCheckCondition> tobeCheckedDomainList) {
            var strList = new List<string>();
            /*
            for (int i = 0; i < checkedListBox_product.Items.Count; i++)
            {
                if (checkedListBox_product.GetItemChecked(i))
                {
                    strList.Add( CommonFunc.FakeProductToRealProduct(checkedListBox_product.Items[i].ToString()));
                }
            }
            */
            LogHelper.Debug("requestDomain start");
            if (string.CompareOrdinal("全部", this._companyName) != 0)
            {
                strList.Add(_companyName);
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
                LogHelper.Debug("strList.Count <= 0");
                AddErrorLog("请先选择至少一个部门.");
                return false;
            }
            AddLog("向服务器请求新域名...");

            var request = new Request();
            var getDomainRequest = new GetDomainRequest { 
                Products = strList.ToArray(),
                CustomIndex = this._customIndex,
                Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV)
            };
            //var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
            var apiPath = Api.GetDomain;
            var re = request.Execute<GetDomainResponse>(apiPath,
                getDomainRequest.ToJson(), "POST");
            LogHelper.Debug("requestDomain请求返回");
            //addLog(getDomainRequest.ToJson());

            if (re is string)
            {
                AddErrorLog("从服务器请求新域名失败...");
                return false;
            }
            else
            {
                LogHelper.Debug("requestDomain re is not string");
                var getDomainResponse = (GetDomainResponse)re;
                //var getDomainResponse = (GetDomainResponse)request.Execute<GetDomainResponse>(apiPath, 
                //    getDomainRequest.ToJson(), "POST");
                if (string.IsNullOrEmpty(getDomainResponse.Error))
                {
                    LogHelper.Debug("requestDomain re is error");
                    if (getDomainResponse.DomainCheckConditions == null)
                    {
                        AddErrorLog("从服务器请求新域名失败...");
                        return false;
                    }
                    int domainCount = getDomainResponse.DomainCheckConditions.Count();
                    for (int i = 0; i < domainCount; i++)
                    {
                        tobeCheckedDomainList.Add(getDomainResponse.DomainCheckConditions[i]);

                        // 垃圾域名
                        tobeCheckedDomainList.Add(TrashDomains.GetRandomTrashDomainCheckCondition());
                    }
                    LogHelper.Debug("requestDomain is ok");
                    return true;
                }
                else
                {
                    AddErrorLog("从服务器请求新域名失败... 错误信息：" + getDomainResponse.Error);
                    return false;
                }
            }
        }

        // 实际监测域名
        private void CheckDomain(IList<DomainCheckCondition> tobeCheckedDomainList, 
            ref int checkedDomainCount,ref int checkFailedDomainCount, int timeout, string username) 
        {
            LogHelper.Debug("checkDomain start");
            if (!tobeCheckedDomainList.Any()) {
                LogHelper.Debug("checkDomain tobeCheckedDomainList.Count() <= 0");
                return;
            }
            checkedDomainCount++;

            AddLog("正在监测第 " + checkedDomainCount.ToString() + " 个域名，已有 " + checkFailedDomainCount + " 个域名检查未通过.");
            DomainCheckCondition condition = tobeCheckedDomainList.First();
            tobeCheckedDomainList.RemoveAt(0);
            if (condition == null)
            {
                LogHelper.Debug("checkDomain condition == null");
                return;
            }

            // 数据加工
            var decryptedCheckDomain = AesHelper.AesDecrypt(condition.Domain, AesHelper.AES_KEY, AesHelper.AES_IV);

            //checkDomain = "https://w6609.net";

            var decryptedCheckPath = AesHelper.AesDecrypt(condition.CheckPath, AesHelper.AES_KEY, AesHelper.AES_IV);
            var decryptedCheckString = AesHelper.AesDecrypt(condition.CheckString, AesHelper.AES_KEY, AesHelper.AES_IV);
            if (!decryptedCheckDomain.StartsWith("http://") && !decryptedCheckDomain.StartsWith("https://"))
                decryptedCheckDomain = "http://" + decryptedCheckDomain;
            if (!decryptedCheckPath.StartsWith("/"))
                decryptedCheckPath = "/" + decryptedCheckPath;


            LogHelper.Debug("checkDomain STEP1：PING");
            // STEP1：PING
            bool isCurlSuccess;
            var isPingBaiduSuccess = false;
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
            catch (Exception e) {
                LogHelper.Error("checkDomain STEP1：PING Exception",e);
                isCurlSuccess = false;
            }

            // 通过百度进行二次复检
            if (!isCurlSuccess) {
                LogHelper.Debug("checkDomain !isCurlSuccess");
                try
                {
                    var p2 = new Ping();
                    var httpUrl2 = new Uri("https://baidu.com");
                    var reply2 = p2.Send(httpUrl2.Host, timeout);
                    if (reply2 != null)
                        switch (reply2.Status)
                        {
                            case IPStatus.Success:
                                // 成功
                                isPingBaiduSuccess = true;
                                LogHelper.Debug("checkDomain isPingBaiduSuccess 成功");
                                break;
                            case IPStatus.TimedOut:
                                // 超时
                                LogHelper.Debug("checkDomain isPingBaiduSuccess 超时");
                                break;
                            default:
                                // 失败
                                LogHelper.Debug("checkDomain isPingBaiduSuccess 失败");
                                break;
                        }
                }
                catch (Exception e)
                {
                    isPingBaiduSuccess = false;
                    LogHelper.Error("checkDomain isPingBaiduSuccess Exception", e);
                }
            }

            if (!isCurlSuccess) {
                if (isPingBaiduSuccess) {
                    AddErrorLog("域名CURL检查失败.");

                    if (!TrashDomains.IsTrashDomain(condition.Domain)) {
                        checkFailedDomainCount++;
                        var request = new Request();
                        var checkDomainResultRequest = new CheckDomainResultRequest
                        {
                            Domain = condition.Domain,
                            CheckIp = CommonFunc.GetIpAddressAndData(true)+CommonFunc.GetIpAddressAndData(true)+"|"+CommonFunc.GetMac(),
                            Creator = username,
                            ClientId = CommonFunc.GetCpuId(),
                            Product = condition.Product,
                            Result = "CURL失败",
                            Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV),
                            Printscreen = new byte[0]
                        };
                       // var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
                        var apiPath = Api.SyncCheckResult;
                        LogHelper.Debug("checkDomain CURL失败数据上报开始");
                        var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                        LogHelper.Debug("checkDomain CURL失败数据上报结束");
                        if (result is string)
                        {
                            AddErrorLog("数据上报失败:" + result);
                        }
                        return;
                    }
                }
                else
                {
                    AddErrorLog("域名CURL失败和百度Ping失败，请检查网络.");
                    return;
                }
            }
            LogHelper.Debug("checkDomain  STEP2: 检查匹配");
            // STEP2: 检查匹配
            if (!string.IsNullOrEmpty(decryptedCheckPath) && !string.IsNullOrEmpty(decryptedCheckString))
            {
                string sResult;
                try
                {
                    sResult = CommonFunc.FindString(decryptedCheckDomain + decryptedCheckPath, decryptedCheckString, timeout);
                }
                catch (Exception e)
                {
                    sResult = "验证失败";
                    LogHelper.Error("checkDomain  STEP2: 检查匹配Exception",e);
                }

                if (string.CompareOrdinal(sResult, "成功") != 0)
                {
                    AddErrorLog("字符串验证失败.");

                    if (!TrashDomains.IsTrashDomain(condition.Domain))
                    {
                        LogHelper.Debug("TrashDomains.IsTrashDomain(condition.Domain)");
                        checkFailedDomainCount++;
                        var request = new Request();
                        decryptedCheckDomain = AesHelper.AesDecrypt(condition.Domain, AesHelper.AES_KEY, AesHelper.AES_IV);
                        if (!decryptedCheckDomain.StartsWith("http://") && !decryptedCheckDomain.StartsWith("https://"))
                            decryptedCheckDomain = "https://" + decryptedCheckDomain;
                        LogHelper.Debug("PrintScreen(decryptedCheckDomain)");
                        var b = PrintScreen(decryptedCheckDomain) ?? new byte[0];
                        var checkDomainResultRequest = new CheckDomainResultRequest
                        {
                            Domain = condition.Domain,
                            CheckIp = CommonFunc.GetIpAddressAndData(true)+CommonFunc.GetIpAddressAndData(true)+"|"+CommonFunc.GetMac(),
                            Creator = username,
                            ClientId = CommonFunc.GetCpuId(),
                            Product = condition.Product,
                            Result = sResult,
                            Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV),
                            Printscreen = b
                        };
                        //var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
                        var apiPath = Api.SyncCheckResult;
                        LogHelper.Debug($"checkDomain {apiPath} 数据上报开始");
                        var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                        LogHelper.Debug($"checkDomain {apiPath} 数据上报结束");
                        if (!(result is string)) return;
                        LogHelper.Debug($"checkDomain {apiPath} 数据上报失败");
                        AddErrorLog("数据上报失败:" + result);
                        return;
                    }
                }
            }
            LogHelper.Debug($"checkDomain STEP3: 全通过");
            // STEP3: 全通过
            {
                if (TrashDomains.IsTrashDomain(condition.Domain)) return;
                LogHelper.Debug($"!TrashDomains.IsTrashDomain(condition.Domain)");
                var request = new Request();
                var checkDomainResultRequest = new CheckDomainResultRequest
                {
                    Domain = condition.Domain,
                    CheckIp = CommonFunc.GetIpAddressAndData(true)+"|"+CommonFunc.GetMac(),
                    Creator = username,
                    ClientId = CommonFunc.GetCpuId(),
                    Product = condition.Product,
                    Result = "成功",
                    Mac = AesHelper.AesEncrypt(CommonFunc.GetMac(), AesHelper.AES_KEY, AesHelper.AES_IV),
                    Printscreen = new byte[0]
                };
                //var ip = IniConfigMgr.IniInstance.LoadConfig("服务器IP");
                var apiPath = Api.SyncCheckResult;
                LogHelper.Debug($"checkDomain {apiPath} 数据上报开始");
                var result = request.Execute<CheckDomainResultResponse>(apiPath, checkDomainResultRequest.ToJson(), "POST");
                LogHelper.Debug($"checkDomain {apiPath} 数据上报结束");
                if (!(result is string)) return;
                LogHelper.Debug($"checkDomain {apiPath} 数据上报失败");
                AddErrorLog("数据上报失败:" + result);
            }
        }

        private static byte[] PrintScreen(string url) {
            LogHelper.Debug($"PrintScreen start");
            var bitmap = WebSiteThumbnail.GetWebSiteThumbnail(url, 800, 600, 400, 300);
            LogHelper.Debug($"PrintScreen bitmap");
            if (bitmap == null) {
                return null;
            }
            LogHelper.Debug($"ImageToByte start");
            var result = CommonFunc.ImageToByte(bitmap);
            LogHelper.Debug($"ImageToByte end");
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
            if (string.CompareOrdinal(button_selectAllProduct.Text, "全选") == 0) {
                for (var i = 0; i < checkedListBox_product.Items.Count; i++)
                {
                    this.checkedListBox_product.SetItemChecked(i, true);
                }
                this.button_selectAllProduct.Text = "取消全选";
            }
            else if (string.CompareOrdinal(button_selectAllProduct.Text, "取消全选") == 0)
            {
                for (var i = 0; i < this.checkedListBox_product.Items.Count; i++)
                {
                    this.checkedListBox_product.SetItemChecked(i, false);
                }
                this.button_selectAllProduct.Text = "全选";
            }
        }
    }
}
