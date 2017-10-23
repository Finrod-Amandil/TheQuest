using System;
using System.Runtime.Serialization;

namespace TheQuest
{
    [Serializable]
    internal class FieldOccupiedException : BoardException
    {
        public FieldOccupiedException()
        {
        }

        public FieldOccupiedException(string message) : base(message)
        {
        }

        public FieldOccupiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FieldOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}