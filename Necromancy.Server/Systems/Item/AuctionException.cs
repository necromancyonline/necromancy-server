using System;
using System.Runtime.Serialization;

namespace Necromancy.Server.Systems.Item
{
    [Serializable]
    public class AuctionException : Exception
    {
        public AuctionException()
        {
            type = AuctionExceptionType.Generic;
        }

        public AuctionException(AuctionExceptionType exceptionType)
        {
            type = exceptionType;
        }

        public AuctionException(string message) : base(message)
        {
        }

        public AuctionException(string message, Exception inner) : base(message, inner)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected AuctionException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public AuctionExceptionType type { get; private set; }
    }
}
