namespace TheQuest
{
    abstract internal class WallFeature
    {
        public WallFeature(Direction wall, int position, FieldAttribute attribute)
        {
            Position = position;
            Wall = wall;
            Attribute = attribute;
        }

        public int Position { get; private set; }
        public Direction Wall { get; private set; }
        public FieldAttribute Attribute { get; set; }
    }
}
