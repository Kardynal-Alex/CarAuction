using System;
using System.Runtime.Serialization;

namespace Auction.BLL.Validation
{
    [Serializable]
    public class AuctionException : Exception
    {
        public AuctionException() { }
        public AuctionException(string message) : base(message) { }
        public AuctionException(string message, Exception innerException) : base(message, innerException) { }
        protected AuctionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
