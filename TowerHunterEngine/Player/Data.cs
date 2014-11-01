namespace TowerHunterEngine.Player
{
    public struct Data
    {
        public int Score { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get { return 100; } }
    }
}
