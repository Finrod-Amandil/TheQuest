using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheQuest
{
    class InventoryIcon : Button
    {
        private int _slotNumber;

        public int SlotNumber
        {
            get { return _slotNumber; }
            set { _slotNumber = value; }
        }

    }
}
