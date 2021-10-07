using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FK域名检测工具管理器
{
    /// <summary>
    /// Create Http Request, using json, and read Http Response.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Url of http server wich request will be created to.
        /// </summary>
        public String URL { get; set; }

        /// <summary>
        /// HTTP Verb wich will be used. Eg. GET, POST, PUT, DELETE.
        /// </summary>
        public String Verb { get; set; }

        /// <summary>
        /// Request content, Json by default.
        /// </summary>
        public String Content
        {
            get { return "application/json"; }
        }

        public HttpWebRequest HttpRequest { get; internal set; }
        public HttpWebResponse HttpResponse { get; internal set; }
        public CookieContainer CookieContainer = new CookieContainer();

        /// <summary>
        /// Constructor Overload that allows passing URL and the VERB to be used.
        /// </summary>
        /// <param name="url">URL which request will be created</param>
        /// <param name="verb">Http Verb that will be userd in this request</param>
        public Request(string url, string verb)
        {
            URL = url;
            Verb = verb;
        }

        /// <summary>
        /// Default constructor overload without any paramter
        /// </summary>
        public Request()
        {
            Verb = "GET";
        }

        public object Execute<TT>(string url, object obj, string verb)
        {
            if (url != null)
                URL = url;

            if (verb != null)
                Verb = verb;

            HttpRequest = CreateRequest();

            try
            {
                WriteStream(obj);
            }
            catch (Exception err)
            {
                return err.ToString();
            }

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }
            catch (Exception err)
            {
                return err.ToString();
            }

            try
            {
                string tmp = ReadResponse();
                object o = JsonConvert.DeserializeObject<TT>(tmp);
                return o;
            }
            catch (Exception err)
            {
                return err.ToString();
            }
        }

        public object Execute<TT>(string url)
        {
            if (url != null)
                URL = url;

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            string tmp = ReadResponse();
            return JsonConvert.DeserializeObject<TT>(tmp);
        }

        public object Execute<TT>()
        {
            if (URL == null)
                throw new ArgumentException("URL cannot be null.");

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            string tmp = ReadResponse();
            return JsonConvert.DeserializeObject<TT>(tmp);
        }

        public object Execute(string url, object obj, string verb)
        {
            if (url != null)
                URL = url;

            if (verb != null)
                Verb = verb;

            HttpRequest = CreateRequest();

            WriteStream(obj);

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        public object Execute(string url)
        {
            if (url != null)
                URL = url;

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        public object Execute()
        {
            if (URL == null)
                throw new ArgumentException("URL cannot be null.");

            HttpRequest = CreateRequest();

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }

            return ReadResponse();
        }

        internal HttpWebRequest CreateRequest()
        {
            var basicRequest = (HttpWebRequest)WebRequest.Create(URL);
            basicRequest.ContentType = Content;
            basicRequest.Method = Verb;
            basicRequest.CookieContainer = CookieContainer;
            basicRequest.Timeout = 10000;

            return basicRequest;
        }

        internal void WriteStream(object obj)
        {
            try
            {
                if (obj != null)
                {
                    using (var streamWriter = new StreamWriter(HttpRequest.GetRequestStream()))
                    {
                        if (obj is string)
                            streamWriter.Write(obj);
                        else
                            streamWriter.Write(JsonConvert.SerializeObject(obj));
                    }
                }
            }
            catch (Exception) { }
        }

        internal String ReadResponse()
        {
            if (HttpResponse != null)
                using (var streamReader = new StreamReader(HttpResponse.GetResponseStream()))
                    return streamReader.ReadToEnd();

            return string.Empty;
        }

        internal String ReadResponseFromError(WebException error)
        {
            using (var streamReader = new StreamReader(error.Response.GetResponseStream()))
                return streamReader.ReadToEnd();
        }

    }
}
