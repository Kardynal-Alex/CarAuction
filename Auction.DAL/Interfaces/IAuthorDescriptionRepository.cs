using Auction.DAL.Entities;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// IAuthorDescriptionRepository
    /// </summary>
    public interface IAuthorDescriptionRepository
    {
        /// <summary>
        /// Add AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        Task AddAuthorDescriptionAsync(AuthorDescription authorDescription);
        /// <summary>
        /// Update AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        void UpdateAuthorDescription(AuthorDescription authorDescription);
        /// <summary>
        /// Get Author Description ByLotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task<AuthorDescription> GetAuthorDescriptionByLotIdAsync(int lotId);
    }
}
