using Auction.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(Favorite addFavorite);
        void DeleteFavorite(string id);
        Task DeleteByLotIdAndUserIdAsync(Favorite deleteFavorite);
        Task<List<Favorite>> GetFavoriteByUserIdAsync(string userId);
        void DeleteInRangeFavorites(List<Favorite> favoriteDTOs);
        Task<Favorite> GetFavoriteByUserIdAndLotIdAsync(Favorite favorite);
    }
}
