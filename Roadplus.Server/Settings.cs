using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;

namespace Roadplus.Server
{
    public class Settings
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public bool EnableHttp { get; set; }
        public bool LogToFile { get; set; }
        public int HttpPort { get; set; }
        public int BaudRate { get; set; }
        public string FileRoot { get; set; }
        public string HttpRoot { get; set; }
        public int RoadDetectTimeOut { get; set; }

        public Settings()
        { }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        public static Settings Load(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            TextReader reader = new StreamReader(path);
            Settings output = (Settings)deserializer.Deserialize(reader);
            reader.Close();
            return output;
        }

        public static Settings GetDefault()
        {
            Settings defaultSettings = new Settings();

            // DEFINE DEFAULT SETTINGS HERE
            defaultSettings.IP = "127.0.0.1";
            defaultSettings.Port = 42424;
            defaultSettings.EnableHttp = true;
            defaultSettings.LogToFile = false;
            defaultSettings.HttpPort = 8080;
            defaultSettings.BaudRate = 19200;
            defaultSettings.FileRoot = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);
            defaultSettings.HttpRoot = Path.Combine(
                defaultSettings.FileRoot, "www/");
            defaultSettings.RoadDetectTimeOut = 300;

            return defaultSettings;
        }
    }
}

