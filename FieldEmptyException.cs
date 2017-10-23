using System;
using System.Runtime.Serialization;

namespace TheQuest
{
    [Serializable]
    internal class FieldEmptyException : BoardException
    {
        public FieldEmptyException()
        {
        }

        public FieldEmptyException(string message) : base(message)
        {
        }

        public FieldEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FieldEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}