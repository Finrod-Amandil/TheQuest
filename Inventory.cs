using System.Collections.Generic;

namespace TheQuest
{
    internal class Inventory
    {
        private int _maxSlots;
        private Dictionary<int, InventorySlot> _slots;

        public Inventory(int maxSlots)
        {
            _maxSlots = maxSlots;
            _slots = new Dictionary<int, InventorySlot>();
            for (int i = 0; i < _maxSlots; i++)
            {
                _slots.Add(i, new InventorySlot());
            }
        }

        public Dictionary<int, InventorySlot> Slots
        {
            get { return _slots; }
        }

        /// <summary>
        /// Tries adding item(s) to the inventory, matching the Inventory insert mode.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="mode">Whether to override items if inventory is full or not.</param>
        public void Add(Item item, InventoryInsertMode mode)
        {
            Add(item, 1, mode);
        }

        /// <summary>
        /// Tries adding item(s) to the inventory, matching the Inventory insert mode.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="amount">How many items to add</param>
        /// <param name="mode">Whether to override items if inventory is full or not.</param>
        public void Add(Item item, int amount, InventoryInsertMode mode)
        {
            //Try to stack item
            if (item is IStackable && IsInInventory(item, out int slot))
            {
                _slots[slot].Add(amount);
            }
            //Else check if there's empty slots and add item to an empty one
            else if (AreEmptySlotsAvailable(out int firstEmptySlot))
            {
                for (int i = 0; i < _maxSlots; i++)
                {
                    if (_slots[i].Item == null)
                    {
                        _slots.Remove(i);
                        _slots.Add(i, new InventorySlot(item, amount));
                        break;
                    }
                }
            }

            else if (mode == InventoryInsertMode.OverrideIfFull)
            {
                _slots[0] = new InventorySlot(item, amount);
            }
            else
            {
                throw new InventoryFullException();
            }
        }

        private bool AreEmptySlotsAvailable(out int firstEmptySlot)
        {
            foreach(int slotNumber in _slots.Keys)
            {
                if(_slots[slotNumber].Item == null)
                {
                    firstEmptySlot = slotNumber;
                    return true;
                }
            }
            firstEmptySlot = 0;
            return false;
        }

        private bool IsInInventory(Item item, out int slot)
        {
            slot = -1;

            foreach (int slotKey in _slots.Keys)
            {
                if (_slots[slotKey].Item == item)
                {
                    slot = slotKey;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all of a specific item from the inventory, if the specified item is available.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(Item item)
        {
            if (IsInInventory(item, out int slot))
            {
                _slots[slot].Remove();
            }
        }
    }
}
