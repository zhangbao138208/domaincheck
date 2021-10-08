using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace FK域名检测工具
{
    /// <summary>
    /// Create Http Request, using json, and read Http Response.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Url of http server wich request will be created to.
        /// </summary>
        private string Url { get; set; }

        /// <summary>
        /// HTTP Verb wich will be used. Eg. GET, POST, PUT, DELETE.
        /// </summary>
        private string Verb { get; set; }

        /// <summary>
        /// Request content, Json by default.
        /// </summary>
        private static string Content => "application/json";

        private HttpWebRequest HttpRequest { get; set; }
        private HttpWebResponse HttpResponse { get; set; }
        public CookieContainer CookieContainer = new CookieContainer();

        /// <summary>
        /// Constructor Overload that allows passing URL and the VERB to be used.
        /// </summary>
        /// <param name="url">URL which request will be created</param>
        /// <param name="verb">Http Verb that will be userd in this request</param>
        public Request(string url, string verb)
        {
            Url = url;
            Verb = verb;
        }

        /// <summary>
        /// Default constructor overload without any paramter
        /// </summary>
        public Request()
        {
            Verb = "POST";
        }

        public object Execute<TT>(string url, object obj, string verb)
        {
            if (url != null)
                Url = url;

            if (verb != null)
                Verb = verb;

            GC.Collect();

            StreamReader sr = null;
            var tmp = "";
            object result;

            HttpRequest = (HttpWebRequest)WebRequest.Create(Url);
            HttpRequest.ContentType = Content;
            HttpRequest.Method = Verb;
            HttpRequest.ProtocolVersion = HttpVersion.Version10;
            HttpRequest.Timeout = 60 * 1000; // 1分钟
            HttpRequest.KeepAlive = false;
            //HttpRequest.ContentLength = obj.ToString().Length;

            ServicePointManager.DefaultConnectionLimit = 100;

            try
            {
                WriteStream(obj);
            }
            catch (Exception err) {
                if (HttpRequest != null)
                {
                    HttpRequest.Abort();
                    HttpRequest = null;
                }

                if (HttpResponse == null) return err.Message;
                HttpResponse.Close();
                HttpResponse = null;
                return err.Message;
            }

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (Exception err) {
                if (HttpRequest != null)
                {
                    HttpRequest.Abort();
                    HttpRequest = null;
                }

                if (HttpResponse == null) return err.ToString();
                HttpResponse.Close();
                HttpResponse = null;
                return err.ToString();
            }

            try
            {
                if (HttpResponse != null)
                {
                    sr = new StreamReader(HttpResponse.GetResponseStream() ?? throw new InvalidOperationException());
                    tmp = sr.ReadToEnd();
                    sr.Dispose();
                    sr.Close();
                    sr = null;
                }

                //MessageBox.Show(tmp);
                result = JsonConvert.DeserializeObject<TT>(tmp);
            }
            catch (Exception err)
            {
                if (HttpRequest != null)
                {
                    HttpRequest.Abort();
                    HttpRequest = null;
                }
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }

                if (HttpResponse == null) return err.ToString();
                HttpResponse.Close();
                HttpResponse = null;
                return err.ToString();
            }

            if (HttpRequest != null)
            {
                HttpRequest.Abort();
                HttpRequest = null;
            }

            if (HttpResponse == null) return result;
            HttpResponse.Close();
            HttpResponse = null;
            return result;
        }

        private void WriteStream(object obj)
        {
            try
            {
                if (obj != null)
                {
                    using (var streamWriter = new StreamWriter(HttpRequest.GetRequestStream()))
                    {
                        //if (obj is string)
                            streamWriter.Write(obj);
                        //else
                        //    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    }
                }
                else {
                    
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /*
        internal String ReadResponseFromError(WebException error)
        {
            var response = error.Response;
            if (response == null) {
                return "No response.";
            }
            Stream s = null;
            try
            {
                s = response.GetResponseStream();
            }
            catch (Exception) {
                return "Get response steam error.";
            }
            var streamReader = new StreamReader(s);
            if (streamReader == null) {
                return "Create stream reader failed.";
            }
            return streamReader.ReadToEnd();
        }
        */

    }
}
