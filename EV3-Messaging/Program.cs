using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using EV3MessengerLib;

namespace EV3_Messaging
{
    class Program
    {
        static EV3Messenger messenger;
        static EV3Message message;

        static string lastmessage = "";

        static void Main(string[] args)
        {
            messenger = new EV3Messenger();
            
            Timer ReadTimer = new Timer(50);
            ReadTimer.Elapsed += new ElapsedEventHandler(ReadTimer_Elapsed);
            ReadTimer.Start();

            if (messenger.Connect("com40"))
            {
                Console.Write("Connected.");
            }
            else
            {
                throw new NotSupportedException("Connection error");
            }

            while (true)
            {
                Console.WriteLine(lastmessage);
            }

        }

        private static void ReadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            message = messenger.ReadMessage();

            if (message != null)
            {
                lastmessage = message.ValueAsText;
            }
        }
    }
}
