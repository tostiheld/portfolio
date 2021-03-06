﻿namespace BombDefuserEngine.Player
{
    public class Data
    {
        public int Score { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; private set; }

        public Data(int maxHP)
        {
            Score = 0;
            HitPoints = maxHP;
            MaxHitPoints = maxHP;
        }
    }
}
