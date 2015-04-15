using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            Console.SetOut(sw);

            Server server = new Server(
                new IPEndPoint(Settings.IP, Settings.Port),
                sw);

            server.Start();

            Console.ReadKey();

            server.Stop();

            Console.ReadKey();
        }
    }
}
