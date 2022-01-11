using NJsonSchema.Annotations;
using System;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// Lot entity.
    /// Contains lot properties.
    /// </summary>
    public class Lot
    {
        /// <summary>
        /// Primary Key
        /// Gets and sets lot id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets and sets lot name.
        /// </summary>
        public string NameLot { get; set; }
        /// <summary>
        /// Gets and sets lot startprice.
        /// </summary>
        public double StartPrice { get; set; }
        /// <summary>
        /// Gets and sets lot is sold.
        /// </summary>
        public bool IsSold { get; set; }
        /// <summary>
        /// Gets and sets iamge path.
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Gets and sets lot description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets and sets user id who creates lot
        /// Foreign Key
        /// </summary>
        public string UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// Gets and sets lot date creation.
        /// </summary>
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// Gets and sets lot current price when customer bids.
        /// </summary>
        public double CurrentPrice { get; set; }
        /// <summary>
        /// Gets and sets lot car year of release.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Gets and sets lotsate about lot info 
        /// </summary>
        public LotState LotState { get; set; }
        /// <summary>
        /// Gets and sets Images
        /// </summary>
        public Images Images { get; set; }
    }
}
