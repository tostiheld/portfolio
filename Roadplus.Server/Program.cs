using System;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Roadplus.Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            Settings settings = new Settings();

            ParseArguments(settings, args);

            Trace.Listeners.Clear();

            if (settings.LogToFile)
            {
                string logfilename =
                    "roadplus-server-" + DateTime.Now.ToString("HHmm-dd-MM-yyyy") + ".log";
                TextWriterTraceListener ftl = 
                    new TextWriterTraceListener(
                        Path.Combine(settings.FileRoot, logfilename));
                ftl.Name = "FileLogger";
                ftl.TraceOutputOptions = TraceOptions.DateTime;
                Trace.Listeners.Add(ftl);
            }

            ConsoleTraceListener ctl = new ConsoleTraceListener(false);
            ctl.TraceOutputOptions = TraceOptions.DateTime;
            Trace.Listeners.Add(ctl);
            Trace.AutoFlush = true;

            RoadplusService service = new RoadplusService(settings);

            service.Start();

            Console.ReadKey();

            service.Stop();

            Console.WriteLine();
            Console.WriteLine("Press a key to close log...");
            Console.ReadKey();
        }

        private static void ParseArguments(Settings settings, string[] args)
        {
            try
            {
                string newHttpRoot = null;

                int i = 0;
                foreach (string s in args)
                {
                    switch (s)
                    {
                        case "-l":
                            settings.LogToFile = true;
                            break;
                        case "-nohttp":
                            settings.EnableHttp = false;
                            break;
                        case "-a":
                            string ip = args[i + 1];
                            IPAddress temp;
                            if (!IPAddress.TryParse(ip, out temp))
                            {
                                throw new ArgumentException("Invalid IP");
                            }
                            settings.IP = temp;
                            break;
                        case "-wsport":
                            int wsport = Convert.ToInt32(args[i + 1]);
                            settings.Port = wsport;
                            break;
                        case "-httpport":
                            int httpport = Convert.ToInt32(args[i + 1]);
                            settings.HttpPort = httpport;
                            break;
                        case "-b":
                            int rate = Convert.ToInt32(args[i + 1]);
                            settings.BaudRate = rate;
                            break;
                        case "-r":
                            string folder = args[i + 1];
                            if (!System.IO.Directory.Exists(folder))
                            {
                                throw new ArgumentException(
                                    "Server root directory not found");
                            }
                            settings.FileRoot = folder;
                            break;
                        case "-httpfolder":
                            newHttpRoot = args[i + 1];
                            break;
                        case "-h":
                        case "-H":
                        case "-help":
                        case "--help":
                            DisplayHelp();
                            Environment.Exit(0);
                            break;
                        default:
                            // do nothing
                        break;
                    }
                    i++;
                }

                if (newHttpRoot != null)
                {
                    string httpfolder = Path.Combine(settings.FileRoot, newHttpRoot);
                    if (!System.IO.Directory.Exists(httpfolder))
                    {
                        throw new ArgumentException(
                            "Http service root directory not found");
                    }
                    settings.HttpRoot = httpfolder;
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
                              "                        Assembly needs to be compiled with the TRACE symbol\n" +
                              "    -nohttp             Disable http service\n" +
                              "    -a [ip]             Server listen ip (default: 127.0.0.1)\n" +
                              "    -wsport [port]      Websockets port (default: 42424)\n" +
                              "    -httpport [port]    Http port (default: 8080)\n" +
                              "    -b [baudrate]       Serial baudrate (must be a valid rate) (default: 38400)\n" +
                              "    -r [path]           Server file root (default: same directory as assembly)\n" +
                              "    -httpfolder [name]  The name of the http root folder (default: www)");
        }
    }
}
