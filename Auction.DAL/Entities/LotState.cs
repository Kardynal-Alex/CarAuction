using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class LotState
    {
        [Key]
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string FutureOwnerId { get; set; }
        public int CountBid { get; set; }
        public int LotId { get; set; }
        public Lot Lot { get; set; }
    }
}
