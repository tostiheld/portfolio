using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;

namespace Roadplus.Server.Communication.Http
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

            string secondprefix = String.Format(
                "http://localhost:{0}/",
                endpoint.Port.ToString());

            listener.Prefixes.Add(prefix);
            listener.Prefixes.Add(secondprefix);
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
                            // we're pretty sure this is a HttpListenerContext so no
                            // type checking is needed
                            HttpListenerContext ctx = c as HttpListenerContext;
                            try
                            {
                                string localpath = MakeLocalPath(ctx.Request);
                                HttpResponse response;
                                if (localpath != null)
                                {
                                    try
                                    {
                                        response = HttpResponse.FromLocalPath(localpath);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex is IOException ||
                                            ex is UnauthorizedAccessException)
                                        {
                                            response = HttpResponse.FromMessage("IO error");
                                        }

                                        throw;
                                    }
                                }
                                else
                                {
                                    response = HttpResponse.FromMessage("ERROR 404: Not found");
                                }

                                byte[] buf = response.Content;
                                ctx.Response.ContentType = response.ContentType;
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
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

        private string MakeLocalPath(HttpListenerRequest request)
        {
            string url = request.RawUrl;

            if (url.Contains(".."))
            {
                return null;
            }
            else if (url == "/")
            {
                url = "./index.html";
            }

            if (!url.StartsWith("."))
            {
                url = "." + url;
            }

            string path = Path.Combine(root, url);

            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                return null;
            }
        }

        public void Stop()
        {
            listener.Stop();
            listener.Close();
        }
    }
}

