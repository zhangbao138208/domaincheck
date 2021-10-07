using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public class WebSiteThumbnail
    {
        Bitmap m_Bitmap;
        string m_Url;
        int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight;
        // ADDED
        public string LastUrl
        {
            get{return _LastUrl;}
            set{_LastUrl = value;}
        }
        private string _LastUrl;
        // END OF ADD

        public WebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            m_Url = Url;
            m_BrowserHeight = BrowserHeight;
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
        }
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            WebSiteThumbnail thumbnailGenerator = new WebSiteThumbnail(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImageSync();
        }

        public Bitmap GenerateWebSiteThumbnailImageSync()
        {
            try
            {
                WebBrowser webBrowser = new WebBrowser();
                webBrowser.ScrollBarsEnabled = false;       //禁用滚动条
                webBrowser.ScriptErrorsSuppressed = true;   //禁用脚本错误
                webBrowser.Navigate(m_Url);
                webBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                webBrowser.NewWindow += new CancelEventHandler(WebBrowser_NewWindow);
                //while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                DateTime before = System.DateTime.Now;
                while (true)
                {
                    if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
                        break;
                    if (webBrowser.ReadyState == WebBrowserReadyState.Interactive && m_Bitmap != null)
                        break;
                    DateTime now = System.DateTime.Now;
                    TimeSpan ts = now.Subtract(before);
                    if(ts.TotalSeconds >= 60)
                    {
                        try
                        {
                            // 强制截图
                            webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                            webBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                            webBrowser.ScrollBarsEnabled = false;
                            m_Bitmap = new Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height);
                            webBrowser.BringToFront();
                            webBrowser.DrawToBitmap(m_Bitmap, webBrowser.Bounds);
                            m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
                        }
                        catch (Exception) { }
                        break;
                    }
                    Application.DoEvents();
                }
                webBrowser.Dispose();
                return m_Bitmap;
            }
            catch (Exception) {
                return null;
            }
        }

        private void WebBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            // ADDED
            //if (m_WebBrowser.ReadyState == WebBrowserReadyState.Complete)
            if (m_WebBrowser.Url.ToString() != LastUrl)
            {
                LastUrl = m_WebBrowser.Url.ToString();
                m_WebBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;
                m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                m_WebBrowser.BringToFront();
                m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
                // 保存图片
                //m_Bitmap.Save(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
            }
        }
    }

}
