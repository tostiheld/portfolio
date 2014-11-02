using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoGameConsole;

namespace BombDefuserEngine.Utils.ConsoleCommands
{
    class SetHP : IConsoleCommand
    {
        Player.Data Data;

        public SetHP(Player.Data data)
        {
            Data = data;
        }

        public string Description
        {
            get { return "Sets the hit points to a specified value.";  }
        }

        public string Execute(string[] arguments)
        {
            int targetValue;
            if (Int32.TryParse(arguments[0], out targetValue))
            {
                if (targetValue > Data.MaxHitPoints)
                {
                    return "Specified value is larger that the maximal amount of hit points (" + Data.MaxHitPoints.ToString() + ").";
                }
                else
                {
                    Data.HitPoints = targetValue;
                    return "Hit points set.";
                }
            }
            else
            {
                return "Value is not a number.";
            }
        }

        public string Name
        {
            get { return "SetHP"; }
        }
    }
}
