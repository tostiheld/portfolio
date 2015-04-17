using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;

namespace Roadplus.Server
{
    public class HttpService
    {
        private readonly HttpListener _listener = new HttpListener();

        public HttpService(params string[] prefixes)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _listener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                                                     {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = SendHttpResponse(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { throw; }
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
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
                string path = Path.Combine(Settings.HttpRoot, url);
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
            _listener.Stop();
            _listener.Close();
        }
    }
}

