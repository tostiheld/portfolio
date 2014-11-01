using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerHunterEngine.Player
{
    public static class Input
    {
        public static Point GetDirections(int scale, int correctionScale)
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);

            float stateX = state.ThumbSticks.Left.X;
            float stateY = state.ThumbSticks.Left.Y;

            int baseSpeed = (int)(stateY * scale);
            int correction = Math.Abs((int)(stateX * correctionScale));

            int leftSpeed = baseSpeed;
            int rightSpeed = baseSpeed;

            if (stateX > 0)
            {
                leftSpeed += correction;
                rightSpeed -= correction;
            }
            else if (stateX < 0)
            {
                leftSpeed -= correction;
                rightSpeed += correction;
            }

            Point p = new Point(leftSpeed, rightSpeed);
            return p;
        }
    }
}
