namespace TheQuest
{
    internal class Target
    {
        private int _translationStraight;
        private int _translationPerpendicular;
        private double _hitChance;

        public Target(int translationStraight, int translationPerpendicular)
        {
            _translationStraight = translationStraight;
            _translationPerpendicular = translationPerpendicular;
            _hitChance = 1D;
        }

        public Target(int translationStraight, int translationPerpendicular, double hitChance)
        {
            _translationStraight = translationStraight;
            _translationPerpendicular = translationPerpendicular;
            _hitChance = hitChance;
        }

        public int TranslationStraight
        {
            get { return _translationStraight; }
        }

        public int TranslationPerpendicular
        {
            get { return _translationPerpendicular; }
        }

        public double HitChance
        {
            get { return _hitChance; }
            set { _hitChance = value; }
        }
    }
}
