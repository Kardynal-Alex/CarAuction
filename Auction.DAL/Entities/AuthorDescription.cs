using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class AuthorDescription
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LotId { get; set; }
        public string Description { get; set; }
    }
}
