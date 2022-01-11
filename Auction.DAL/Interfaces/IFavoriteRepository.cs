using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// IFavoriteRepository
    /// </summary>
    public interface IFavoriteRepository
    {
        /// <summary>
        /// Add favorite entity
        /// </summary>
        /// <param name="addFavorite"></param>
        /// <returns></returns>
        Task AddFavoriteAsync(Favorite addFavorite);
        /// <summary>
        /// Delete favorite by if
        /// </summary>
        /// <param name="id"></param>
        void DeleteFavorite(string id);
        /// <summary>
        /// Delete by lot id and user id
        /// </summary>
        /// <param name="deleteFavorite"></param>
        Task DeleteByLotIdAndUserIdAsync(Favorite deleteFavorite);
        /// <summary>
        /// Get favorites by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Favorite>> GetFavoriteByUserIdAsync(string userId);
        /// <summary>
        /// Delete user favorite lots
        /// </summary>
        /// <param name="userId"></param>
        void DeleteInRangeFavorites(List<Favorite> favoriteDTOs);
        /// <summary>
        /// Get favorite by userid and lotid
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        Task<Favorite> GetFavoriteByUserIdAndLotIdAsync(Favorite favorite);
    }
}
