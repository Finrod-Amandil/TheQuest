namespace TheQuest
{
    internal class HealingPotion : Potion
    {
        public HealingPotion() : this("Healing Potion") { }

        public HealingPotion(string name) : base(name, 8) { }
    }
}
