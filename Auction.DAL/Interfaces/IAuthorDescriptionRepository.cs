using Auction.DAL.Entities;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface IAuthorDescriptionRepository
    {
        Task AddAuthorDescriptionAsync(AuthorDescription authorDescription);
        void UpdateAuthorDescription(AuthorDescription authorDescription);
        Task<AuthorDescription> GetAuthorDescriptionByLotIdAsync(int lotId);
    }
}
