using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.WepApi.Models
{
    public class LotStateViewModel
    {
        /// <summary>
        /// LotStateViewModel id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LotStateViewModel owner id who create lot.
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// LotStateViewModel futureowner id who bids lot.
        /// </summary>
        public string FutureOwnerId { get; set; }
        /// <summary>
        /// LotStateViewModel count number of bids
        /// </summary>
        public int CountBid { get; set; }
        /// <summary>
        /// LotStateViewModel lot id
        /// </summary>
        public int LotId { get; set; }
    }
}
