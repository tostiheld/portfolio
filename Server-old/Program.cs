using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            ParseArguments(args);

            Trace.Listeners.Clear();

            if (Settings.LogToFile)
            {
                string logfilename =
                    "roadplus-server-" + DateTime.Now.ToString("HHmm-dd-MM-yyyy") + ".log";
                TextWriterTraceListener ftl = 
                    new TextWriterTraceListener(
                        Path.Combine(Settings.FileRoot, logfilename));
                ftl.Name = "FileLogger";
                ftl.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;
                Trace.Listeners.Add(ftl);
            }

            ConsoleTraceListener ctl = new ConsoleTraceListener(false);
            ctl.TraceOutputOptions = TraceOptions.DateTime;
            Trace.Listeners.Add(ctl);
            Trace.AutoFlush = true;

            Server server = new Server(
                new IPEndPoint(Settings.IP, Settings.Port));

            server.Start();

            Console.ReadKey();

            server.Stop();

            Console.WriteLine();
            Console.WriteLine("Press a key to close log...");
            Console.ReadKey();
        }

        private static void ParseArguments(string[] args)
        {
            try
            {
                string newHttpRoot = null;
                string newZonePath = null;

                int i = 0;
                foreach (string s in args)
                {
                    switch (s)
                    {
                        case "-l":
                            Settings.LogToFile = true;
                            break;
                        case "-nohttp":
                            Settings.EnableHttp = false;
                            break;
                        case "-a":
                            string ip = args[i + 1];
                            if (!IPAddress.TryParse(ip, out Settings.IP))
                            {
                                throw new ArgumentException("Invalid IP");
                            };
                            break;
                        case "-wsport":
                            int wsport = Convert.ToInt32(args[i + 1]);
                            Settings.Port = wsport;
                            break;
                        case "-httpport":
                            string httpport = args[i + 1];
                            if (Convert.ToInt64(httpport) > Int32.MaxValue)
                            {
                                throw new ArgumentOutOfRangeException(
                                    "Port number must be lower than " + Int32.MaxValue.ToString());
                            }
                            Settings.HttpPort = httpport;
                            break;
                        case "-b":
                            int rate = Convert.ToInt32(args[i + 1]);
                            Settings.BaudRate = rate;
                            break;
                        case "-r":
                            string folder = args[i + 1];
                            if (!System.IO.Directory.Exists(folder))
                            {
                                throw new ArgumentException(
                                    "Server root directory not found");
                            }
                            Settings.FileRoot = folder;
                            break;
                        case "-httpfolder":
                            newHttpRoot = args[i + 1];
                            break;
                        case "-zones":
                            newZonePath = args[i + 1];
                            break;
                        case "-h":
                        case "-H":
                        case "-help":
                        case "--help":
                            DisplayHelp();
                            Environment.Exit(0);
                            break;
                        default:
                            // just run with default settings
                            break;
                    }
                    i++;
                }

                Settings.HttpServiceUrl = "http://" + Settings.IP.ToString() + ":" + Settings.HttpPort + "/";
                if (newHttpRoot != null)
                {
                    string httpfolder = Path.Combine(Settings.FileRoot, newHttpRoot);
                    if (!System.IO.Directory.Exists(httpfolder))
                    {
                        throw new ArgumentException(
                            "Http service root directory not found");
                    }
                    Settings.HttpRoot = httpfolder;
                }

                if (newZonePath != null)
                {
                    Settings.ZoneFilePath = Path.Combine(Settings.FileRoot, newZonePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DisplayHelp();
                Environment.Exit(1);
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Usage: roadplusserver.exe [parameters]\n\n" +

                              "    Possible parameters:\n" +
                              "    -l                  Log to file (default: false)\n" +
                              "                        Log file located in the same folder as the server assembly\n" +
                              "                        Filename is roadplus-server-<start-time-and-date>.log\n" +
                              "    -nohttp             Disable http service\n" +
                              "    -a [ip]             Server listen ip (default: 127.0.0.1)\n" +
                              "    -wsport [port]      Websockets port (default: 42424)\n" +
                              "    -httpport [port]    Http port (default: 8080)\n" +
                              "    -b [baudrate]       Serial baudrate (must be a valid rate) (default: 38400)\n" +
                              "    -r [path]           Server file root (default: same directory as assembly)\n" +
                              "    -httpfolder [name]  The name of the http root folder (default: www)\n" +
                              "    -zones [name]       Zone file name (located in server root) (default: default.zones)");
        }
    }
}
