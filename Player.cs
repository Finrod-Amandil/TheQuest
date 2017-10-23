using System.Collections.Generic;

namespace TheQuest
{
    internal class Player : Character
    {
        private Inventory _inventory;

        public Player(int inventorySize) : base(10)
        {
            _inventory = new Inventory(inventorySize);
        }

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Tries to pick up an item from the board and add it to the player's inventory.
        /// </summary>
        /// <param name="item">The item to pick up</param>
        /// <param name="hasPickedUp">Out parameter: Whether item was able to be picked up</param>
        public void PickUpItem(Item item, out bool hasPickedUp)
        {
            try
            {
                _inventory.Add(item, InventoryInsertMode.DoNotAddIfFull);
                hasPickedUp = true;
                item.IsSpawned = false;
                item.IsInInventory = true;
            }
            catch(InventoryFullException ife)
            {
                hasPickedUp = false;
            }
        }

        /// <summary>
        /// Returns a list of Targets the player can hit with the current weapon.
        /// </summary>
        /// <param name="weapon">The weapon to attack with</param>
        /// <returns>List of Target objects, representing the relative distance from the player to the targets.</returns>
        public virtual List<Target> Attack(Weapon weapon)
        {
            return weapon.Targets;
        }

        /// <summary>
        /// Consumes a potion
        /// </summary>
        /// <param name="potion">The potion to use</param>
        public void ConsumePotion(Potion potion)
        {
            HitPoints = HitPoints + potion.HitPoints > HitPointsMax ? HitPointsMax : HitPoints + potion.HitPoints;
            potion.Destroy();
            _inventory.Remove(potion);
        }
    }
}
