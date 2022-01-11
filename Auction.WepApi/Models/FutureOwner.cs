
namespace Auction.WepApi.Models
{
    /// <summary>
    /// <see cref="FutureOwner"/> is not db class
    /// Needed for inputing complicated data
    /// </summary>
    public class FutureOwner
    {
        /// <summary>
        /// Lot id
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// Owner Id
        /// </summary>
        public string OwnerId { get; set; }
    }
}
