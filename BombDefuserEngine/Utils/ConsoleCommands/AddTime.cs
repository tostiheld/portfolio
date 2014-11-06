using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoGameConsole;

namespace BombDefuserEngine.Utils.ConsoleCommands
{
    public class AddTime :IConsoleCommand
    {
        PlayerFeedback.CountdownTimer cTimer;

        public AddTime(PlayerFeedback.CountdownTimer ctimer)
        {
            cTimer = ctimer;
        }

        public string Description
        {
            get { return "Adds or removes time (in seconds) from the countdown. Tick tock."; }
        }

        public string Execute(string[] arguments)
        {
            int time = 0;
            
            if (Int32.TryParse(arguments[0], out time))
            {
                cTimer.TimeLeft += TimeSpan.FromSeconds(time);
                return "Time added";
            }
            else
            {
                return "NaN";
            }
        }

        public string Name
        {
            get { return "AddTime"; }
        }
    }
}
