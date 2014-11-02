namespace BombDefuserEngine.Player
{
    public class Data
    {
        public int Score { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; private set; }

        public Data(int maxHP)
        {
            Score = 0;
            HitPoints = 0;
            MaxHitPoints = maxHP;
        }

        /*
        public void SetHP(int HP)
        {
            if (HP > MaxHitPoints)
            {
                return;
            }
            else
            {
                HitPoints = HP;
            }
        }*/
    }
}
