using System;
using System.Runtime.Serialization;

namespace Auction.BLL.Validation
{
    /// <summary>
	/// Auction Exception
	/// </summary>
    [Serializable]
    public class AuctionException : Exception
    {
        /// <summary>
		/// AuctionException ctor
		/// </summary>
        public AuctionException() { }
        /// <summary>
		/// AuctionException ctor
		/// </summary>
		/// <param name="message"></param>
        public AuctionException(string message) : base(message) { }
        /// <summary>
        /// AuctionException ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AuctionException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>
        /// AuctionException ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AuctionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        /// <summary>
		/// Get Object Data
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
