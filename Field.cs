namespace TheQuest
{
    internal class Field
    {
        private Character _character;
        private Item _item;
        private int _indexX;
        private int _indexY;
        private FieldAttribute _attribute;

        public Field(int indexX, int indexY)
        {
            _indexX = indexX;
            _indexY = indexY;
            _attribute = FieldAttribute.None;
        }

        public Character Character
        {
            get { return _character; }
            set { _character = value; }
        }
        public Item Item
        {
            get { return _item; }
            set { _item = value; }
        }
        public int IndexX
        {
            get { return _indexX; }
            set { _indexX = value; }
        }
        public int IndexY
        {
            get { return _indexY; }
            set { _indexY = value; }
        }
        public FieldAttribute Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
    }
}
