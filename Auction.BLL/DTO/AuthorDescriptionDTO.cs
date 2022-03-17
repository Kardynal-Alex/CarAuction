
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="AuthorDescriptionDTO"/> data transfer class
    /// </summary>
    public class AuthorDescriptionDTO
    {
        /// <summary>
        /// AuthorDescriptionDTO Id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// AuthorDescriptionDTO UserId.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// AuthorDescriptionDTO LotId.
        /// </summary>
        public int LotId { get; set; }
        /// <summary>
        /// AuthorDescriptionDTO Description.
        /// </summary>
        public string Description { get; set; }
    }
}
