
using System;

namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="FavoriteDTO"/> data transfer class
    /// </summary>
    public class FavoriteDTO
    {
        /// <summary>
        /// FavoriteDTO Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// FavoriteDTO UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// FavoriteDTO LotId
        /// </summary>
        public int LotId { get; set; }
    }
}
