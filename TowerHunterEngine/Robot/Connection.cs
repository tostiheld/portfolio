using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using TowerHunterEngine.Utils.Exceptions;

using EV3MessengerLib;

namespace TowerHunterEngine.Robot
{
    public class Connection
    {
        private EV3Messenger Messenger;
        private Timer ReadTimer;

        public EV3Message LastMessage { get; private set; }

        public Connection(string port)
        {
            Messenger = new EV3Messenger();
            if (!Messenger.Connect(port))
                throw new EV3CommunicationException(
                    "Connection with EV3 failed");

            ReadTimer = new Timer(1);
            ReadTimer.Elapsed += new ElapsedEventHandler(ReadTimer_Elapsed);
            ReadTimer.Start();
        }

        private void ReadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Messenger != null || Messenger.IsConnected)
            {
                EV3Message message = Messenger.ReadMessage();
                if (message != null)
                {
                    this.LastMessage = message;
                }
            }
            else
            {
                throw new EV3CommunicationException(
                    "Connection interrupted while reading message.");
            }
        }

        public void SendWheelData(int leftWheel, int RightWeel)
        {
            if (Messenger != null || Messenger.IsConnected)
            {
                if (Messenger.SendMessage("left", (float)leftWheel) &&
                    Messenger.SendMessage("right", (float)RightWeel))
                    return;
                else
                    throw new EV3CommunicationException(
                        "Failed to send wheel data.");
            }
            else
            {
                throw new EV3CommunicationException(
                    "Not connected to the EV3.");
            }
        }
    }
}
