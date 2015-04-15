using System;
using System.Net;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    class MainClass
    {
        private static List<WSSession> sessions;

        public static void Main(string[] args)
        {
            sessions = new List<WSSession>();

            Console.Write("Startign ws... ");
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 4242);
            WSService service = new WSService(ip);
            service.NewSession += Service_NewSession;
            service.Start();
            Console.WriteLine("ws started.");
            Console.ReadLine();

            foreach (WSSession ws in sessions)
            {
                ws.End();
            }

            service.Stop();
        }

        private static void Service_NewSession(object sender, NewSessionEventArgs e)
        {
            sessions.Add(e.Session);
            e.Session.MessageReceived += Session_NewMessage;
            e.Session.Disconnected += Session_Disconnected;
            Console.WriteLine("New session");
        }

        private static void Session_Disconnected(object sender, EventArgs e)
        {
            WSSession session = sender as WSSession;
            Console.WriteLine("Client at " + session.IP.ToString() + " disconnected");
        }

        private static void Session_NewMessage(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Received.ToString());
        }
    }
}
