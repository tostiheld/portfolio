using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TowerHunterEngine.Utils.Exceptions;

using EV3MessengerLib;

namespace TowerHunterEngine.Robot
{
    public class Connection
    {
        private EV3Messenger Messenger;

        public Connection(string port)
        {
            Messenger = new EV3Messenger();
            if (!Messenger.Connect(port))
                throw new EV3CommunicationException("Connection with EV3 failed");
        }

        public void SendWheelData(int leftWheel, int RightWeel)
        {
            if (Messenger != null || Messenger.IsConnected)
            {
                if (Messenger.SendMessage("left", (float)leftWheel) &&
                    Messenger.SendMessage("right", (float)RightWeel))
                    return;
                else
                    throw new EV3CommunicationException("Failed to send wheel data.");
            }
            else
            {
                throw new EV3CommunicationException("Not connected to the EV3.");
            }
        }

        public RobotStatus GetStatus()
        {
            if (Messenger != null || Messenger.IsConnected)
            {
                EV3Message message = Messenger.ReadMessage();
                if (message != null)
                {
                    switch (message.ValueAsText)
                    {
                        case "hitwall":
                            return RobotStatus.HitWall;
                        case "finished":
                            return RobotStatus.Finished;
                        case "getcoin":
                            return RobotStatus.GetCoin;
                        default:
                            return RobotStatus.Empty;
                    }
                }
                else
                {
                    return RobotStatus.Empty;
                }
            }
            else
            {
                throw new EV3CommunicationException("Not connected to the EV3.");
            }
        }
    }
}
