using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Controller_test
{
    class Program
    {
        static void Main(string[] args)
        {
            int ctop = Console.CursorTop;
            int cleft = Console.CursorLeft;

            while (true)
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                Console.SetCursorPosition(cleft, ctop);
                Console.Write("                                                          ");
                Console.SetCursorPosition(cleft, ctop);
                Console.Write(String.Format("X: {0} ", state.ThumbSticks.Left.X.ToString()));
                Console.Write(String.Format("Y: {0}", state.ThumbSticks.Left.Y.ToString()));

                Thread.Sleep(10);
            }
        }
    }
}
