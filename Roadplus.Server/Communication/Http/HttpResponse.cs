using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Roadplus.Server.Communication.Http
{
    public class HttpResponse
    {
        public int ContentLength 
        { 
            get
            {
                return Content.Length;
            }
        }

        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public HttpResponse(byte[] content, string extension)
        {
            ContentType = MimeTypeMap.GetMimeType(extension);
            Content = content;
        }

        public static HttpResponse FromLocalPath(string path)
        {
            string extension = Path.GetExtension(path);
            byte[] content = File.ReadAllBytes(path);
            return new HttpResponse(content, extension);
        }

        public static HttpResponse FromMessage(string message)
        {
            string extension = ".txt";
            byte[] content = Encoding.UTF8.GetBytes("Road+ HTTP service\n\n" + message);
            return new HttpResponse(content, extension);
        }
    }
}
