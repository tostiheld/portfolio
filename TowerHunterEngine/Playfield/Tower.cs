using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerHunterEngine.Playfield
{
    public class Tower
    {
        public Tower(Square target)
        {
            this.Active = true;
            this.Target = target;
        }

        public bool Active { get; private set; }
        public Square Target { get; private set; }

        public void SetState(bool state)
        {
            if (state)
            {
                this.Active = true;
                Target.ChangeType(SquareType.Forbidden);
            }
            else if (!state)
            {
                this.Active = false;
                Target.ChangeType(SquareType.Safe);
            }
        }
    }
}
