namespace TheQuest
{
    internal class InventorySlot
    {
        private Item _item;
        private int _count;

        public InventorySlot()
        {
            _item = null;
            _count = 0;
        }

        public InventorySlot(Item item)
        {
            _item = item;
            _count = 1;
        }

        public InventorySlot(Item item, int count)
        {
            _item = item;
            _count = count;
        }

        public Item Item { get { return _item; } }
        public int Count { get { return _count; } }

        /// <summary>
        /// Increases the amount of the item in this slot by 1.
        /// </summary>
        public void Add()
        {
            Add(1);
        }

        /// <summary>
        /// Increases the amount of the item in this slot by the specified amount
        /// </summary>
        /// <param name="amount">How many items to add</param>
        public void Add(int amount)
        {
            _count += amount;
        }

        /// <summary>
        /// Removes 1 item from the slot. Throws NotEnoughItemsException if not enough items are available.
        /// </summary>
        public void Remove()
        {
            Remove(1);
        }

        /// <summary>
        /// Removes the specified amount of items from the slot. Throws NotEnoughItemsException if
        /// not enough items are available.
        /// </summary>
        /// <param name="amount">The amount of items to remove</param>
        public void Remove(int amount)
        {
            if (_count >= amount)
            {
                _count -= amount;
                if (_count == 0)
                {
                    _item = null;
                }
            }
            else if(_count == 0)
            {
                throw new NotEnoughItemsException("Slot is empty!");
            }
            else
            {
                throw new NotEnoughItemsException("Not enough items!");
            }
        }
    }
}
