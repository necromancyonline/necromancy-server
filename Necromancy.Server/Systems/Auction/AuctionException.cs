using System;

namespace Necromancy.Server.Systems.Auction
{
    [Serializable()]
    public class AuctionException : Exception
    {
        public AuctionExceptionType Type { get; private set; }
        public AuctionException() : base() { Type = AuctionExceptionType.Generic; }
        public AuctionException(AuctionExceptionType exceptionType) : base() { Type = exceptionType; }
        public AuctionException(string message) : base(message) { }
        public AuctionException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected AuctionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        
    }
}
