namespace Auction.BLL.DTO
{
    /// <summary>
    /// LotStateDTO
    /// </summary>
    public class LotStateDTO
    {
        /// <summary>
        /// LotStateDTO id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LotStateDTO owner id who create lot.
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// LotStateDTO futureowner id who bids lot.
        /// </summary>
        public string FutureOwnerId { get; set; }
        /// <summary>
        /// LotStateDTO count number of bids
        /// </summary>
        public int CountBid { get; set; }
        /// <summary>
        /// LotStateDTO lot id
        /// </summary>
        public int LotId { get; set; }
    }
}