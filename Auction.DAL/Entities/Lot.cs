using NJsonSchema.Annotations;
using System;
using System.Text.Json.Serialization;

namespace Auction.DAL.Entities
{
    public class Lot
    {
        public int Id { get; set; }
        public string NameLot { get; set; }
        public double StartPrice { get; set; }
        public bool IsSold { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public CarBrand CarBrand { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }
        public double CurrentPrice { get; set; }
        public int Year { get; set; }
        public LotState LotState { get; set; }
        public Images Images { get; set; }
    }
}
