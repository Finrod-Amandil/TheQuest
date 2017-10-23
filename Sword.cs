using System.Collections.Generic;

namespace TheQuest
{
    internal class Sword : Weapon
    {
        public Sword() : this("Sword", 4, 1) { } //Default sword

        public Sword(string name, int damagePoints, int maxHits) 
            : base(name, damagePoints, maxHits)
        {

        }

        public override List<Target> Targets {
            get
            {
                return new List<Target>()
                {
                    new Target(1, 0, 0.9D),
                    new Target(0, 1, 0.7D),
                    new Target(0, -1, 0.7D)
                };
            }
        }
    }
}
