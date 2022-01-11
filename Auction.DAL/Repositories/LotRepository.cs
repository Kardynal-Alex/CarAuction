using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// Lot Repository
    /// </summary>
    public class LotRepository : ILotRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public LotRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
		/// Add lot entity
		/// </summary>
        /// <param name="addLot"></param>
        public async Task AddLotAsync(Lot addLot)
        {
            await context.Lots.AddAsync(addLot);
        }
        /// <summary>
		/// Delete entity
		/// </summary>
        /// <param name="lot"></param>
        public void DeleteLot(Lot lot)
        {
            context.Lots.Remove(lot);
        }
        /// <summary>
        /// Delete lot by lot id
        /// </summary>
        /// <param name="lotId"></param>
        public void DeleteLotByLotId(int lotId)
        {
            context.Remove(new Lot { Id = lotId });
        }
        /// <summary>
        /// Delete user lots
        /// </summary>
        /// <param name="lots"></param>
        public void DeleteLotsRange(List<Lot> lots)
        {
            context.RemoveRange(lots);
        }
        /// <summary>
        /// Get all lots entity
        /// </summary>
        public async Task<List<Lot>> GetAllLotsAsync()
        {
            return await context.Lots.Where(x => x.IsSold == false).AsNoTracking().ToListAsync();
        }
        /// <summary>
        ///  Get favorite lots by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Lot>> GetFavoriteLotsByUserIdAsync(string userId)
        {
            var list = from lst1 in context.Lots
                       from lst2 in context.Favorites
                       where lst2.UserId == userId && lst1.Id == lst2.LotId
                       select lst1;
            return await list.ToListAsync();
        }
        /// <summary>
        /// Get fresh lot which are added today and yestarday
        /// </summary>
        /// <returns></returns>
        public async Task<List<Lot>> GetFreshLots()
        {
            var count = context.Lots.Count();
            return (count < 6 ?
                await context.Lots.AsNoTracking().ToListAsync() :
                await context.Lots.AsNoTracking().Skip(count - 6).ToListAsync());
        }
        /// <summary>
        /// Get lot by id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task<Lot> GetLotByIdAsync(int lotId)
        {
            return await context.Lots.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == lotId);
        }
        /// <summary>
        /// Get lot by id entity with details about user and lotsate
        /// </summary>
        /// <param name="lotId"></param>
        public async Task<Lot> GetLotByIdWithDetailsAsync(int lotId)
        {
            return await context.Lots.Where(x => x.Id == lotId)
                                     .Include(x => x.User)
                                     .Include(x => x.LotState)
                                     .Include(x => x.Images)
                                     .FirstOrDefaultAsync();
        }
        /// <summary>
		/// Get lots by user id
		/// </summary>
        /// <param name="userId"></param>
        public async Task<List<Lot>> GetLotsByUserIdAsync(string userId)
        {
            return await context.Lots.Where(x => x.UserId == userId)
                                     .Include(x => x.User)
                                     .Include(x => x.LotState)
                                     .ToListAsync();
        }
        /// <summary>
        /// Get sold lots
        /// </summary>
        /// <returns></returns>
        public async Task<List<Lot>> GetSoldLotsAsync()
        {
            return await context.Lots.Where(x => x.IsSold == true).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get user bids(all lots where he made bid(can be sold lot and no yet))
        /// </summary>
        /// <param name="futureOwnerId"></param>
        public async Task<List<Lot>> GetUserBidsAsync(string futureOwnerId)
        {
            var list = from lst1 in context.Lots.Include(x => x.User)
                       from lst2 in context.LotStates
                       where lst2.FutureOwnerId == futureOwnerId && lst1.Id == lst2.LotId
                       && lst2.OwnerId != futureOwnerId
                       select lst1;
            return await list.ToListAsync();
        }
        /// <summary>
		/// Update lot entity
		/// </summary>
        /// <param name="updateLot"></param>
        public void UpdateLot(Lot updateLot)
        {
            context.Entry(updateLot).State = EntityState.Modified;
        }
    }
}
