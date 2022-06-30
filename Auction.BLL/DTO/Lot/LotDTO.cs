using Auction.BLL.DTO.Identity;
using NJsonSchema.Annotations;
using System;

namespace Auction.BLL.DTO.Lot
{
    public class LotDTO
    {
        public int Id { get; set; }
        public string NameLot { get; set; }
        public double StartPrice { get; set; }
        public bool IsSold { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public CarBrandDTO CarBrand { get; set; }
        public UserDTO User { get; set; }
        public string UserId { get; set; }
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        public double CurrentPrice { get; set; }
        public int Year { get; set; }
        public LotStateDTO LotState { get; set; }
        public ImagesDTO Images { get; set; }
    }
}
