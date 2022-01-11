
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="FutureOwnerDTO"/> is not db class
    /// Needed for inputing complicated data
    /// </summary>
    public class FutureOwnerDTO
    {  
        /// <summary>
        /// FutureOwnerDTO LotId
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// FutureOwnerDTO OwnerId
        /// </summary>
        public string OwnerId { get; set; }
    }
}
