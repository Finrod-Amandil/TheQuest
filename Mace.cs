using System.Collections.Generic;

namespace TheQuest
{
    internal class Mace : Weapon
    {
        public Mace() : this("Mace", 2, -1) { }

        public Mace(string name, int damagePoints, int maxHits) 
            : base(name, damagePoints, maxHits) { }

        public override List<Target> Targets
        {
            get
            {
                List<Target> retVal = new List<Target>();
                for (int i = -2; i <= 2; i++)
                {
                    for (int j = -2; j <= 2; j++)
                    {
                        if (j == 0 && i == 0) continue;
                        retVal.Add(new Target(i, j, 0.8D));
                    }
                }
                return retVal;
            }
        }
    }
}
