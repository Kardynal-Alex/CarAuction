using Auction.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(FavoriteDTO favoriteDTO);
        Task DeleteFavoriteAsync(string id);
        Task<List<FavoriteDTO>> GetFavoriteByUserIdAsync(string userId);
        Task DeleteFavoriteByLotIdAndUserIdAsync(FavoriteDTO favoriteDTO);
        Task DeleteInRangeFavoritesAsync(List<FavoriteDTO> favoriteDTOs);
        Task<FavoriteDTO> GetFavoriteByUserIdAndLotIdAsync(FavoriteDTO favoriteDTO);
    }
}
