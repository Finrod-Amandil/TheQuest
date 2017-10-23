namespace TheQuest
{
    internal class Door : WallFeature
    {
        public Door(Direction wall, int position) : base(wall, position, FieldAttribute.PlayerSpawn) { }
    }
}
