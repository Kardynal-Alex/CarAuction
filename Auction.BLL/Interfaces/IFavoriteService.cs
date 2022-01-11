using Auction.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// IFavoriteService
    /// </summary>
    public interface IFavoriteService
    {
        /// <summary>
        /// Add favorite
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        Task AddFavoriteAsync(FavoriteDTO favoriteDTO);
        /// <summary>
        /// delete favorite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteFavoriteAsync(string id);
        /// <summary>
        /// Get favorite by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<FavoriteDTO>> GetFavoriteByUserIdAsync(string userId);
        /// <summary>
        /// Delete favorite by lot id and user id
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        Task DeleteFavoriteByLotIdAndUserIdAsync(FavoriteDTO favoriteDTO);
        /// <summary>
        /// Delete user favorite lots
        /// </summary>
        /// <param name="userId"></param>
        Task DeleteInRangeFavoritesAsync(List<FavoriteDTO> favoriteDTOs);
        /// <summary>
        /// Get favorite by user if and lot id
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        Task<FavoriteDTO> GetFavoriteByUserIdAndLotIdAsync(FavoriteDTO favoriteDTO);
    }
}
