using System.Linq;

namespace TheQuest
{
    abstract internal class Item : IMoveable
    {
        private Field _field;
        private string _name;
        private bool _isInInventory;
        private bool _isSpawned;

        public Item(string name)
        {
            _name = name;
        }

        public Item()
        {
            _name = null;
        }

        public Field Field
        {
            get { return _field; }
            set { _field = value; }
        }

        public string Name
        {
            get { return _name == null ? GetType().ToString().Split(new char[] { '.' }).Last() : _name; }
            set { _name = value; }
        }

        public bool IsInInventory
        {
            get { return _isInInventory; }
            set { _isInInventory = value; }
        }
        public bool IsSpawned
        {
            get { return _isSpawned; }
            set { _isSpawned = value; }
        }

        /// <summary>
        /// Sets the item's field to the specified item.
        /// </summary>
        /// <param name="field">The field to place the item on.</param>
        public void MoveTo(Field field)
        {
            _field = field;
        }

        /// <summary>
        /// Destroys the item by setting its IsSpawned and IsInventory status to false.
        /// </summary>
        public void Destroy()
        {
            _isInInventory = false;
            _isSpawned = false;
        }
    }
}
