using System.Collections.Generic;

namespace TheQuest
{
    internal class Bow : Weapon, IStackable
    {
        private int _fireBonusDamage;

        public Bow() : this("Bow", 3) { }

        public Bow(string name, int damagePoints) : base(name, damagePoints, 1)
        {
            _fireBonusDamage = 10;
        }

        public int FireBonusDamage
        {
            get { return _fireBonusDamage; }
        }

        public override List<Target> Targets
        {
            get
            {
                List<Target> retVal = new List<Target>();
                for (int i = 1; i <= 10; i++)
                {
                    retVal.Add(new Target(i, 0, 1D - ((double)(i+1)) / 10));
                }
                return retVal;
            }
        }
    }
}
