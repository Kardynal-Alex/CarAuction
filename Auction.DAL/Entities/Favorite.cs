using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// Favorite entity.
    /// Contains favorite properties.
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// primary key
        /// Gets and sets favorite id.
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// Gets and sets favorite UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets and sets favorite LotId
        /// </summary>
        public int LotId { get; set; }
    }
}
