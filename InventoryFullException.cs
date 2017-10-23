using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheQuest
{
    class InventoryFullException : Exception
    {
        public InventoryFullException() : base() { }
        public InventoryFullException(string message) : base(message) { }
        public InventoryFullException(string message, Exception e) : base(message, e) { }
    }
}
