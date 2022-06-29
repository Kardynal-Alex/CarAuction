using Auction.WepApi.Models.Identity;
using NJsonSchema.Annotations;
using System;

namespace Auction.WepApi.Models.Lot
{
    public class LotViewModel
    {
        public int Id { get; set; }
        public string NameLot { get; set; }
        public double StartPrice { get; set; }
        public bool IsSold { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public CarBrandViewModel CarBrand { get; set; }
        public UserViewModel User { get; set; }
        public string UserId { get; set; }
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        public double CurrentPrice { get; set; }
        public int Year { get; set; }
        public LotStateViewModel LotState { get; set; }
        public ImagesViewModel Images { get; set; }
    }
}
