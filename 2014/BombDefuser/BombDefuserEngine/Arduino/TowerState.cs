using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerHunterEngine.Arduino
{
    public class TowerState
    {
        public TowerState(int towerAmount)
        {
            this.Activated = new List<bool>(towerAmount);
            this.Hit = new List<bool>(towerAmount);
        }

        public List<bool> Activated { get; set; }
        public List<bool> Hit { get; set; }
    }
}
