using Auction.BLL.DTO;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// IAuthorDescriptionService
    /// </summary>
    public interface IAuthorDescriptionService
    {
        /// <summary>
        /// Add AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        Task AddAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO);
        /// <summary>
        /// Update AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        Task UpdateAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO);
        /// <summary>
        /// Get Author Description ByLotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task<AuthorDescriptionDTO> GetAuthorDescriptionByLotIdAsync(int lotId);
    }
}
