using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EV3MessengerLib;

namespace Besturing_Controller
{
    class Program
    {
        static int SCALE = 40;
        static int CORRECTION_SCALE = 20;
        static string PORT = "com40";

        static void Main(string[] args)
        {
            try
            {
                EV3Messenger messenger = new EV3Messenger();

                if (messenger.Connect(PORT))
                {
                    Console.WriteLine("Connected");
                }
                else
                {
                    throw new InvalidOperationException("Connection failed");
                }

                int ctop = Console.CursorTop;
                int cleft = Console.CursorLeft;
                while (true)
                {
                    GamePadState state = GamePad.GetState(PlayerIndex.One);

                    float stateX = state.ThumbSticks.Left.X;
                    float stateY = state.ThumbSticks.Left.Y;

                    int baseSpeed = (int)(stateY * SCALE);
                    int correction = Math.Abs((int)(stateX * CORRECTION_SCALE));

                    int leftSpeed = baseSpeed;
                    int rightSpeed = baseSpeed;

                    if (stateX > 0)
                    {
                        leftSpeed += correction;
                        rightSpeed -= correction;
                    }
                    else if (stateX < 0)
                    {
                        leftSpeed -= correction;
                        rightSpeed += correction;
                    }

                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write("                         ");
                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write(leftSpeed.ToString() + " " + rightSpeed.ToString());

                    messenger.SendMessage("left", (float)leftSpeed);
                    messenger.SendMessage("right", (float)rightSpeed);

                    Thread.Sleep(50); // sleep so the brick can keep up
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
