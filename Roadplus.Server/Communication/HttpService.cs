using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;

namespace Roadplus.Server.Communication
{
    public class HttpService
    {
        private HttpListener listener;
        private string root;

        public HttpService(IPEndPoint endpoint, string httproot)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException(
                    "HttpListener is not supported on this platform");
            }
            else if (endpoint == null)
            {
                throw new ArgumentException("endpoint");
            }
            else if (httproot == null)
            {
                throw new ArgumentNullException("httproot");
            }

            listener = new HttpListener();

            string prefix = String.Format(
                "http://{0}:{1}/",
                endpoint.Address.ToString(),
                endpoint.Port.ToString());

            listener.Prefixes.Add(prefix);
            root = httproot;
        }

        public void Start()
        {
            // TODO: make this method less shit
            listener.Start();

            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            HttpListenerContext ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = SendHttpResponse(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch 
                            {
                                throw new NotImplementedException();
                            }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, listener.GetContext());
                    }
                }
                catch (HttpListenerException)
                {
                    // ignore exception on close
                }
            });
        }

        private string SendHttpResponse(HttpListenerRequest request)
        {
            string url = request.RawUrl;

            if (url.Contains(".."))
            {
                return "Illegal request";
            }
            else if (url == "/")
            {
                url = "./index.html";
            }

            if (!url.StartsWith("."))
            {
                url = "." + url;
            }

            try
            {
                string path = Path.Combine(root, url);
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                return "404 - not found";
            }
        }

        public void Stop()
        {
            listener.Stop();
            listener.Close();
        }
    }
}

