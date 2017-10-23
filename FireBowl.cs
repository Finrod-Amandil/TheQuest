namespace TheQuest
{
    internal class FireBowl : WallFeature
    {
        public FireBowl(Direction wall, int position) : base(wall, position, FieldAttribute.FireBonus) { }
    }
}
