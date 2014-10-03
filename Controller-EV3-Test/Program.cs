using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoBrick.EV3;

namespace Controller_EV3_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            byte[] left = new byte[1]{(byte)'L'};
            byte[] right = new byte[1]{(byte)'R'};
            byte[] niks = new byte[1] { (byte)'X' };

            try
            {
                var brick = new Brick<Sensor, Sensor, Sensor, Sensor>("usb");
                brick.Connection.Open();
                Console.WriteLine("Connected");

                do
                {
                    GamePadState state = GamePad.GetState(PlayerIndex.One);
                    //cki = Console.ReadKey(true);

                    if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        brick.Mailbox.Send("mailbox1", left);
                    }
                    else if (state.Buttons.RightShoulder == ButtonState.Pressed)
                    {
                        brick.Mailbox.Send("mailbox1", right);
                    }
                    else
                    {
                        brick.Mailbox.Send("mailbox1", niks);
                    }

                } while (true); //while (cki.Key != ConsoleKey.Q);

                brick.Connection.Close();
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
    }
}
