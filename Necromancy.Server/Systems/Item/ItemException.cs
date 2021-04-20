using System;
using System.Runtime.Serialization;

namespace Necromancy.Server.Systems.Item
{
    [Serializable]
    public class ItemException : Exception
    {
        public ItemException()
        {
            type = ItemExceptionType.Generic;
        }

        public ItemException(ItemExceptionType exceptionType)
        {
            type = exceptionType;
        }

        public ItemException(string message) : base(message)
        {
        }

        public ItemException(string message, Exception inner) : base(message, inner)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected ItemException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public ItemExceptionType type { get; private set; }
    }
}
