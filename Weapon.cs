using System;
using System.Collections.Generic;

namespace TheQuest
{
    abstract internal class Weapon : Item
    {
        private int _damagePoints;
        private int _maxHits;

        public Weapon(string name, int damagePoints, int maxHits) : base(name)
        {
            _damagePoints = damagePoints;
            _maxHits = maxHits;
        }

        public int DamagePoints
        {
            get { return _damagePoints; }
        }

        public int MaxHits
        {
            get { return _maxHits; }
        }

        public abstract List<Target> Targets { get; }

        private int TargetDistance(Target target)
        {
            return Math.Abs(target.TranslationStraight) + Math.Abs(target.TranslationPerpendicular);
        }

        /// <summary>
        /// Returns a description of the weapon's characteristics.
        /// </summary>
        /// <returns>A multi-line string describing the weapon.</returns>
        public override string ToString()
        {
            string description = $"{Name.ToUpperInvariant()}\r\n";
            description += $"Damage: {_damagePoints}\r\n";
            if (_maxHits == -1)
            {
                description += "Can hit all enemies within range!\r\n";
            }
            else if (_maxHits > 1)
            {
                description += $"Can hit up to {_maxHits} enemies at once!\r\n";
            }

            return description;
        }
    }
}
