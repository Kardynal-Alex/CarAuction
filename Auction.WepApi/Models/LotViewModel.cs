using NJsonSchema.Annotations;
using System;

namespace Auction.WepApi.Models
{
    /// <summary>
    /// Lot View Model
    /// </summary>
    public class LotViewModel
    {
        /// <summary>
        /// LotViewModel Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LotViewModel NameLot
        /// </summary>
        public string NameLot { get; set; }
        /// <summary>
        /// LotViewModel StartPrice
        /// </summary>
        public double StartPrice { get; set; }
        /// <summary>
        /// LotViewModel Is Sold Lot
        /// </summary>
        public bool IsSold { get; set; }
        /// <summary>
        /// LotViewModel image path
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// LotViewModel Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// LotViewModel User property
        /// </summary>
        public UserViewModel User { get; set; }
        /// <summary>
        /// LotViewModel UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// LotViewModel start date of creation lot
        /// </summary>
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// LotViewModel current price after bids
        /// </summary>
        public double CurrentPrice { get; set; }
        /// <summary>
        /// LotViewModel lot car year of release.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// LotViewModel LotState property
        /// </summary>
        public LotStateViewModel LotState { get; set; }
        /// <summary>
        /// LotViewModel Images Property
        /// </summary>
        public ImagesViewModel Images { get; set; }
    }
}
