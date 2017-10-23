using System;

namespace TheQuest
{
    class NotEnoughItemsException : Exception
    {
        public NotEnoughItemsException() : base() { }
        public NotEnoughItemsException(string message) : base(message) { }
        public NotEnoughItemsException(string message, Exception e) : base(message, e) { }
    }
}
