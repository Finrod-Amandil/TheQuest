using System.Collections.Generic;

namespace TheQuest
{
    internal class Bomb : Weapon, IStackable
    {
        private int _throwDistance;

        public Bomb() : this("Bomb", 6) { }

        public Bomb(string name, int damagePoints) : base(name, damagePoints, -1)
        {
            _throwDistance = 3;
        }
        public override List<Target> Targets
        {
            get
            {
                return new List<Target>();
            }
        }
    }
}
