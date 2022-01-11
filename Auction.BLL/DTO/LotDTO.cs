using NJsonSchema.Annotations;
using System;

namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="LotDTO"/> data transfer class
    /// </summary>
    public class LotDTO
    {
        /// <summary>
        /// LotDTO Id(primary key)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LotDTO NameLot
        /// </summary>
        public string NameLot { get; set; }
        /// <summary>
        /// LotDTO Start Price
        /// </summary>
        public double StartPrice { get; set; }
        /// <summary>
        /// LotDTO Is sold lot
        /// </summary>
        public bool IsSold { get; set; }
        /// <summary>
        /// LotDTO Image Path
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// LotDTO Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// LotDTO User
        /// <see cref="UserDTO"/> reference
        /// </summary>
        public UserDTO User { get; set; }
        /// <summary>
        /// LotDTO UserId(foreing key)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// LotDTO Start DateTime
        /// </summary>
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// LotDTO CurrentLotPrice after bids
        /// </summary>
        public double CurrentPrice { get; set; }
        /// <summary>
        /// LotDTO lot car year of release.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// LotDTO LotState
        /// <see cref="LotStateDTO"/> reference
        /// </summary>
        public LotStateDTO LotState { get; set; }
        /// <summary>
        /// LotDTO Images
        /// <see cref="ImagesDTO"/> reference
        /// </summary>
        public ImagesDTO Images { get; set; }
    }
}
