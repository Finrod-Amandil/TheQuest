using System.Collections.Generic;

namespace TheQuest
{
    internal class Axe : Weapon
    {
        public Axe() : this("Axe", 3, 1) { }

        public Axe(string name, int damagePoints, int maxHits) 
            : base(name, damagePoints, maxHits) {
        }

        public override List<Target> Targets
        {
            get
            {
                return new List<Target>()
                {
                    new Target(1, 0, 0.9D),
                    new Target(0, 1, 0.9D),
                    new Target(0, -1, 0.9D),
                    new Target(-1, 0, 0.9D),

                    new Target(1, 1, 0.7D),
                    new Target(1, -1, 0.7D),
                    new Target(-1, 1, 0.7D),
                    new Target(-1, -1, 0.7D)
                };
            }
        }
    }
}
