using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EV3MessengerLib;
using System.Threading;

namespace Controller_EV3_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int ctop;
            int cleft;

            //Timer readTimer = new Timer(new TimerCallback(Timer);

            try
            {
                EV3Messenger messenger = new EV3Messenger();

                if (messenger.Connect("com40"))
                {
                    Console.WriteLine("Connected.");
                    ctop = Console.CursorTop;
                    cleft = Console.CursorLeft;
                }
                else
                    throw new InvalidOperationException("Connection error.");

                while (true)
                {
                    EV3Message licht = messenger.ReadMessage();
                    GamePadState state = GamePad.GetState(PlayerIndex.One);
                    int left = 0;
                    int right = 0;

                    int baseSpeed = (int)(state.Triggers.Left * 60);
                    int baseSpeed = (int)(state.Triggers.Left * 40);
                    int message = baseSpeed * 100;
                    message += baseSpeed;

                    if (state.ThumbSticks.Left.X >= 0)
                        right = (int)(state.ThumbSticks.Left.X * 30);
                    else if (state.ThumbSticks.Left.X <= 0)
                        left = (int)Math.Abs(state.ThumbSticks.Left.X * 30);
                        right = (int)(state.ThumbSticks.Left.X * 50);
                    else if (state.ThumbSticks.Left.X <= 0)
                        left = (int)Math.Abs(state.ThumbSticks.Left.X * 50);

                    right *= 100;
                    message += left;
                    message += right;

                    if (state.Buttons.A == ButtonState.Pressed)
                        message = -1;

                    int turretspeed = (int)(state.ThumbSticks.Right.X * 20);

                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write("                           ");
                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write(message.ToString() + " " + turretspeed.ToString());
                    Console.Write(message.ToString() + " " + turretspeed.ToString() + " " + licht.ToString());

                    messenger.SendMessage("sturen", (float)message);
                    messenger.SendMessage("turret", (float)turretspeed);

                    //Thread.Sleep(new TimeSpan(1000));
                    Thread.Sleep(50);
                    /* Brick can be slow processing bluetooth commands and queues them,
                     * so simply add a delay of 50ms in each loop in the sender and the
                     * brick can handle messages fine, brick has been tested without
                     * any noticable delay and brick 'code' doesn't need waits.
                     */
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Connection closed.");
                Console.ReadKey();
            }
        }

        private void TimerOperation(object state)
        {

        }
    }
}
