using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using EV3MessengerLib;
using Microsoft.Xna.Framework;

using BombDefuserEngine.Utils;
using BombDefuserEngine.Utils.Exceptions;

namespace BombDefuserEngine.Robot
{
    public class Connection
    {
        private EV3Messenger Messenger;
        private Timer ReadTimer;

        public EV3Message LastMessage { get; private set; }
        public Color LastColor { get; private set; }
        public RobotStatus LastStatus { get; private set; }

        public Connection(string port)
        {
            Messenger = new EV3Messenger();
            if (!Messenger.Connect(port))
                throw new EV3CommunicationException(
                    "Connection with EV3 failed");

            this.LastStatus = RobotStatus.Empty;
            this.LastColor = Color.White;

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
                    switch (message.ValueAsText)
                    {
                        case "red":
                            this.LastColor = RobotColors.Red;
                            break;
                        case "green":
                            this.LastColor = RobotColors.Green;
                            break;
                        case "blue":
                            this.LastColor = RobotColors.Blue;
                            break;
                        case "cyan":
                            this.LastColor = RobotColors.Cyan;
                            break;
                        case "yellow":
                            this.LastColor = RobotColors.Yellow;
                            break;
                        case "magenta":
                            this.LastColor = RobotColors.Magenta;
                            break;
                        case "orange":
                            this.LastColor = RobotColors.Orange;
                            break;
                        case "dismantling":
                            this.LastStatus = RobotStatus.Dismantling;
                            break;
                        case "homing":
                            this.LastStatus = RobotStatus.Homing;
                            break;
                        case "nothing":
                            this.LastStatus = RobotStatus.Empty;
                            break;
                        default:
                            // iets anders dan een kleur of status
                            break;
                    }
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
