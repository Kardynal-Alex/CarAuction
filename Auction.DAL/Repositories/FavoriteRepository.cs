using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationContext context;
        public FavoriteRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
     
        public async Task AddFavoriteAsync(Favorite addFavorite)
        {
            await context.Favorites.AddAsync(addFavorite);
        }
      
        public async Task DeleteByLotIdAndUserIdAsync(Favorite deleteFavorite)
        {
            var favorite = await context.Favorites.
                FirstOrDefaultAsync(x => x.LotId == deleteFavorite.LotId && x.UserId == deleteFavorite.UserId);
            context.Remove(favorite);
        }
       
        public void DeleteFavorite(string id)
        {
            context.Favorites.Remove(new Favorite { Id = id });
        }
       
        public void DeleteInRangeFavorites(List<Favorite> favorites)
        {
            context.Favorites.RemoveRange(favorites);
        }
     
        public async Task<Favorite> GetFavoriteByUserIdAndLotIdAsync(Favorite favorite)
        {
            return await context.Favorites.FirstOrDefaultAsync(x => x.UserId == favorite.UserId && x.LotId == favorite.LotId);
        }
     
        public async Task<List<Favorite>> GetFavoriteByUserIdAsync(string userId)
        {
            return await context.Favorites.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
        }
    }
}
