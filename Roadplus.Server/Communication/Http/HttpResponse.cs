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
        private const string OkResponse = "HTTP/1.1 200 OK\r\n" +
                                          "Content-Type: {0}\r\n" +
                                          "Content-Length: {1}\r\n\r\n";

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
            byte[] content = Encoding.UTF8.GetBytes(message);
            return new HttpResponse(content, extension);
        }

        public string Header()
        {
            return String.Format(
                OkResponse,
                ContentType,
                ContentLength.ToString());
        }

        public byte[] ToByteArray()
        {
            byte[] header = Encoding.UTF8.GetBytes(Header());
            byte[] response = new byte[header.Length + Content.Length];
            Array.Copy(
                header, 
                response, 
                header.Length);
            Array.Copy(
                Content, 
                0,
                response,
                header.Length, 
                Content.Length);

            return response;
        }
    }
}
