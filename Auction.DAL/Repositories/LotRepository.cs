using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Repositories
{
    public class LotRepository : ILotRepository
    {
        private readonly ApplicationContext context;
        public LotRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
    
        public async Task AddLotAsync(Lot addLot)
        {
            await context.Lots.AddAsync(addLot);
        }
      
        public void DeleteLot(Lot lot)
        {
            context.Lots.Remove(lot);
        }
      
        public void DeleteLotByLotId(int lotId)
        {
            context.Remove(new Lot { Id = lotId });
        }
      
        public void DeleteLotsRange(List<Lot> lots)
        {
            context.RemoveRange(lots);
        }
       
        public async Task<List<Lot>> GetAllLotsAsync()
        {
            return await context.Lots.Where(x => x.IsSold == false).AsNoTracking().ToListAsync();
        }
     
        public async Task<List<Lot>> GetFavoriteLotsByUserIdAsync(string userId)
        {
            var list = from lst1 in context.Lots
                       from lst2 in context.Favorites
                       where lst2.UserId == userId && lst1.Id == lst2.LotId
                       select lst1;
            return await list.ToListAsync();
        }
       
        public async Task<List<Lot>> GetFreshLots()
        {
            var count = context.Lots.Count();
            return (count < 6 ?
                await context.Lots.AsNoTracking().ToListAsync() :
                await context.Lots.AsNoTracking().Skip(count - 6).ToListAsync());
        }
       
        public async Task<Lot> GetLotByIdAsync(int lotId)
        {
            return await context.Lots.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == lotId);
        }
       
        public async Task<Lot> GetLotByIdWithDetailsAsync(int lotId)
        {
            return await context.Lots
                .Where(x => x.Id == lotId)
                    .Include(x => x.User)
                        .Include(x => x.LotState)
                            .Include(x => x.Images)
                                .FirstOrDefaultAsync();
        }
       
        public async Task<List<Lot>> GetLotsByUserIdAsync(string userId)
        {
            return await context.Lots
                .Where(x => x.UserId == userId)
                    .Include(x => x.User)
                        .Include(x => x.LotState)
                            .ToListAsync();
        }
       
        public async Task<List<Lot>> GetSoldLotsAsync()
        {
            return await context.Lots.Where(x => x.IsSold == true).AsNoTracking().ToListAsync();
        }
    
        public async Task<List<Lot>> GetUserBidsAsync(string futureOwnerId)
        {
            var list = from lst1 in context.Lots.Include(x => x.User)
                       from lst2 in context.LotStates
                       where lst2.FutureOwnerId == futureOwnerId && lst1.Id == lst2.LotId
                       && lst2.OwnerId != futureOwnerId
                       select lst1;
            return await list.ToListAsync();
        }
       
        public void UpdateLot(Lot updateLot)
        {
            context.Entry(updateLot).State = EntityState.Modified;
        }
    }
}
