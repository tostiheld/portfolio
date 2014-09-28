using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerHunterEngine.Playfield
{
    public class Tower
    {
        public Tower(int lastID, Point target)
        {
            this.ID = lastID + 1;
            this.Active = true;
            this.Target = target;
        }

        public int ID { get; private set; }
        public bool Active { get; set; }
        public Point Target { get; private set; }
        
        public void Activate(ref Field field)
        {
            this.Active = true;
            field.Grid[Target.X, Target.Y].ChangeType(SquareType.Forbidden);
        }

        public void Deactivate(ref Field field)
        {
            this.Active = false;
            field.Grid[Target.X, Target.Y].ChangeType(SquareType.Safe);
        }
    }
}
