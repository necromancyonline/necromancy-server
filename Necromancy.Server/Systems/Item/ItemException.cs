using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    [Serializable()]
    public class ItemException :  Exception
    {
        public ItemExceptionType Type { get; private set; }
        public ItemException() : base() { Type = ItemExceptionType.Generic; }
        public ItemException(ItemExceptionType exceptionType) : base() { Type = exceptionType; }
        public ItemException(string message) : base(message) { }
        public ItemException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected ItemException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
