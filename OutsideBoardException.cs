using System;
using System.Runtime.Serialization;

namespace TheQuest
{
    [Serializable]
    internal class OutsideBoardException : BoardException
    {
        public OutsideBoardException()
        {
        }

        public OutsideBoardException(string message) : base(message)
        {
        }

        public OutsideBoardException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutsideBoardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}