namespace TheQuest
{
    internal class Portal : WallFeature
    {
        public Portal(Direction wall, int position) : base(wall, position, FieldAttribute.EnemySpawn) { }
    }
}
