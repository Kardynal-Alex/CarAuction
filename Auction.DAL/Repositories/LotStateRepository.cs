using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class LotStateRepository : ILotStateRepository
    {
        private readonly ApplicationContext context;
        public LotStateRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
    
        public void AddLotState(LotState addLotState)
        {
            context.LotStates.Add(addLotState);
        }
     
        public async Task DeleteLotStateByLotIdAsync(int lotId)
        {
            var lotState = await context.LotStates.FirstOrDefaultAsync(x => x.LotId == lotId);
            context.LotStates.Remove(lotState);
        }
     
        public void DeleteLotStatesRange(List<LotState> lotStates)
        {
            context.RemoveRange(lotStates);
        }
      
        public async Task<LotState> FindLotStateByLotIdAsync(int lotId)
        {
            return await context.LotStates.FirstOrDefaultAsync(x => x.LotId == lotId);
        }
     
        public async Task<List<LotState>> GetAllLotStateAsync()
        {
            return await context.LotStates.ToListAsync();
        }
       
        public async Task<List<LotState>> GetUserLotstatesAsync(string userId)
        {
            return await context.LotStates.Where(x => x.OwnerId == userId).ToListAsync();
        }
     
        public void UpdateLotState(LotState lotState)
        {
            context.LotStates.Update(lotState);
        }
    }
}
