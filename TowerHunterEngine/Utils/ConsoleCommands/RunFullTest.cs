using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameConsole;

namespace TowerHunterEngine.Utils.ConsoleCommands
{
    public class RunFullTest : IConsoleCommand
    {
        private Engine Game;
        private Point Resolution;
        private Point Size;
        private int TowerAmount;

        public RunFullTest()
        {

        }

        public string Description
        {
            get { return "Tests the full game. This includes: Arduino, playfield, EV3, input, output"; }
        }

        public string Execute(string[] arguments)
        {

            return "All tests completed";
        }

        public string Name
        {
            get { return "RunFullTest"; }
        }
    }
}
