using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    /// <summary>
    /// AuthorDescription entity.
    /// </summary>
    public class AuthorDescription
    {
        /// <summary>
        /// Primary Key
        /// Gets and sets AuthorDescription id.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets and sets AuthorDescription UserId.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets and sets AuthorDescription LotId.
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// Gets and sets AuthorDescription Description.
        /// </summary>
        public string Description { get; set; }
    }
}
