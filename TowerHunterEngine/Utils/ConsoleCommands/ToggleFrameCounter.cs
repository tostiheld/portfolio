using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoGameConsole;
using Microsoft.Xna.Framework;

namespace BombDefuserEngine.Utils.ConsoleCommands
{
    class ToggleFrameCounter : IConsoleCommand
    {
        private Engine Parent;

        public ToggleFrameCounter(Engine parent)
        {
            Parent = parent;
        }

        public string Description
        {
            get { return "Toggles the display of the fps counter."; }
        }

        public string Execute(string[] arguments)
        {
            if (Parent.HasFrameCounter)
            {
                foreach (GameComponent gc in Parent.Components)
                {
                    if (gc.GetType() == typeof(FrameCounter))
                    {
                        Parent.Components.Remove(gc);
                        Parent.HasFrameCounter = false;
                        return "Successfully hidden the FPS counter.";
                    }
                }
            }
            else if (!Parent.HasFrameCounter)
            {
                Parent.Components.Add(new FrameCounter(Parent));
                Parent.HasFrameCounter = true;
                return "Successfully shown the FPS counter.";
            }

            return "Error.";
        }

        public string Name
        {
            get { return "ToggleFrameCounter"; }
        }
    }
}
