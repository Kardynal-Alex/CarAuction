using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class Favorite
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public int LotId { get; set; }
    }
}
