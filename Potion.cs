namespace TheQuest
{
    abstract class Potion : Item
    {
        private int _hitPoints;

        public int HitPoints
        {
            get { return _hitPoints; }
            set { _hitPoints = value; }
        }

        public Potion(string name, int hitPoints) : base(name)
        {
            _hitPoints = hitPoints;
        }
    }
}
