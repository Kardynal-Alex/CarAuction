using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// Lot entity.
    /// Contains lotstate properties.
    /// </summary>
    public class LotState
    {
        /// <summary>
        /// Primary Key
        /// Gets and sets lotsate id.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets and sets owner id who create lot.
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// Gets and sets futureowner id who bids lot.
        /// </summary>
        public string FutureOwnerId { get; set; }
        /// <summary>
        /// Count number of bids
        /// </summary>
        public int CountBid { get; set; }
        /// <summary>
        /// Gets and sets lot id
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// Gets and sets Lot for conection one to one
        /// </summary>
        public Lot Lot { get; set; }
    }
}
