using Auction.BLL.DTO.Lot;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    public interface IAuthorDescriptionService
    {
        Task AddAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO);
        Task UpdateAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO);
        Task<AuthorDescriptionDTO> GetAuthorDescriptionByLotIdAsync(int lotId);
    }
}
