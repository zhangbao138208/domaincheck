using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FK域名检测工具
{
    public class WebSiteThumbnail
    {
        private Bitmap _mBitmap;
        private readonly string _mUrl;
        private readonly int _mBrowserWidth;

        private readonly int _mBrowserHeight;
        private readonly int _mThumbnailWidth;
        private readonly int _mThumbnailHeight;

        // ADDED
        private string LastUrl { get; set; }

        // END OF ADD

        private WebSiteThumbnail(string url, int browserWidth, int browserHeight, int thumbnailWidth, int thumbnailHeight)
        {
            _mUrl = url;
            _mBrowserHeight = browserHeight;
            _mBrowserWidth = browserWidth;
            _mThumbnailWidth = thumbnailWidth;
            _mThumbnailHeight = thumbnailHeight;
        }
        public static Bitmap GetWebSiteThumbnail(string url, int browserWidth, int browserHeight, int thumbnailWidth, int thumbnailHeight)
        {
            var thumbnailGenerator = new WebSiteThumbnail(url, browserWidth, browserHeight, thumbnailWidth, thumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImageSync();
        }

        private Bitmap GenerateWebSiteThumbnailImageSync()
        {
            try
            {
                var webBrowser = new WebBrowser();
                webBrowser.ScrollBarsEnabled = false;       //禁用滚动条
                webBrowser.ScriptErrorsSuppressed = true;   //禁用脚本错误
                webBrowser.Navigate(_mUrl);
                webBrowser.ClientSize = new Size(_mBrowserWidth, _mBrowserHeight);
                webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
                webBrowser.NewWindow += WebBrowser_NewWindow;
                //while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
                var before = DateTime.Now;
                while (true)
                {
                    if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
                        break;
                    if (webBrowser.ReadyState == WebBrowserReadyState.Interactive && _mBitmap != null)
                        break;
                    var now = DateTime.Now;
                    var ts = now.Subtract(before);
                    if(ts.TotalSeconds >= 60)
                    {
                        try
                        {
                            // 强制截图
                            webBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
                            webBrowser.ClientSize = new Size(_mBrowserWidth, _mBrowserHeight);
                            webBrowser.ScrollBarsEnabled = false;
                            _mBitmap = new Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height);
                            webBrowser.BringToFront();
                            webBrowser.DrawToBitmap(_mBitmap, webBrowser.Bounds);
                            _mBitmap = (Bitmap)_mBitmap.GetThumbnailImage(_mThumbnailWidth, _mThumbnailHeight, null, IntPtr.Zero);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        break;
                    }
                    Application.DoEvents();
                }
                webBrowser.Dispose();
                return _mBitmap;
            }
            catch (Exception) {
                return null;
            }
        }

        private static void WebBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var mWebBrowser = (WebBrowser)sender;
            // ADDED
            //if (m_WebBrowser.ReadyState == WebBrowserReadyState.Complete)
            if (mWebBrowser.Url.ToString() == LastUrl) return;
            LastUrl = mWebBrowser.Url.ToString();
            mWebBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
            mWebBrowser.ClientSize = new Size(_mBrowserWidth, _mBrowserHeight);
            mWebBrowser.ScrollBarsEnabled = false;
            _mBitmap = new Bitmap(mWebBrowser.Bounds.Width, mWebBrowser.Bounds.Height);
            mWebBrowser.BringToFront();
            mWebBrowser.DrawToBitmap(_mBitmap, mWebBrowser.Bounds);
            _mBitmap = (Bitmap)_mBitmap.GetThumbnailImage(_mThumbnailWidth, _mThumbnailHeight, null, IntPtr.Zero);
            // 保存图片
            //m_Bitmap.Save(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
        }
    }

}
