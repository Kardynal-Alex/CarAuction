using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// Favorite Repository
    /// </summary>
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public FavoriteRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
        /// Add favorite
        /// </summary>
        /// <param name="addFavorite"></param>
        /// <returns></returns>
        public async Task AddFavoriteAsync(Favorite addFavorite)
        {
            await context.Favorites.AddAsync(addFavorite);
        }
        /// <summary>
        /// Delete by lot id and user id
        /// </summary>
        /// <param name="deleteFavorite"></param>
        public async Task DeleteByLotIdAndUserIdAsync(Favorite deleteFavorite)
        {
            var favorite = await context.Favorites.
                FirstOrDefaultAsync(x => x.LotId == deleteFavorite.LotId && x.UserId == deleteFavorite.UserId);
            context.Remove(favorite);
        }
        /// <summary>
        /// Delete favorite by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFavorite(string id)
        {
            context.Favorites.Remove(new Favorite { Id = id });
        }
        /// <summary>
        /// Delete user favorite lots
        /// </summary>
        /// <param name="favoriteDTOs"></param>
        public void DeleteInRangeFavorites(List<Favorite> favorites)
        {
            context.Favorites.RemoveRange(favorites);
        }
        /// <summary>
        /// Get favorite by userid and lotid
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<Favorite> GetFavoriteByUserIdAndLotIdAsync(Favorite favorite)
        {
            return await context.Favorites.FirstOrDefaultAsync(x => x.UserId == favorite.UserId && x.LotId == favorite.LotId);
        }
        /// <summary>
        /// get user favorite lots by userId 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Favorite>> GetFavoriteByUserIdAsync(string userId)
        {
            return await context.Favorites.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
        }
    }
}
