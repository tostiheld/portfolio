using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Roadplus.Server
{
    public class Settings
    {
        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public bool EnableHttp { get; set; }
        public bool LogToFile { get; set; }
        public int HttpPort { get; set; }
        public int BaudRate { get; set; }
        public string FileRoot { get; set; }
        public string HttpRoot { get; set; }
        public int RoadDetectTimeOut { get; set; }

        public Settings()
        {
            IP = IPAddress.Any;
            Port = 42424;
            EnableHttp = true;
            LogToFile = false;
            HttpPort = 8080;
            BaudRate = 38400;
            FileRoot = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);
            HttpRoot = Path.Combine(FileRoot, "www/");
            RoadDetectTimeOut = 300;
        }
    }
}

