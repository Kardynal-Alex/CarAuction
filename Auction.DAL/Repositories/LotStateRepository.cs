using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// LotState Repository
    /// </summary>
    public class LotStateRepository : ILotStateRepository
    {
        private readonly ApplicationContext context;
        /// <summary>
        /// Repository ctor
        /// </summary>
        public LotStateRepository(ApplicationContext ctx)
        {
            context = ctx;
        }
        /// <summary>
        /// Add lotstate entity
        /// </summary>
        public void AddLotState(LotState addLotState)
        {
            context.LotStates.Add(addLotState);
        }
        /// <summary>
        /// Delete lotstate entity by id
        /// </summary>
        /// <param name="lotId"></param>
        public async Task DeleteLotStateByLotIdAsync(int lotId)
        {
            var lotState = await context.LotStates.FirstOrDefaultAsync(x => x.LotId == lotId);
            context.LotStates.Remove(lotState);
        }
        /// <summary>
        /// Delete user lotstates in range
        /// </summary>
        /// <param name="lotStates"></param>
        public void DeleteLotStatesRange(List<LotState> lotStates)
        {
            context.RemoveRange(lotStates);
        }
        /// <summary>
        /// Find lotstate entity by lotid
        /// </summary>
        /// <param name="lotId"></param>
        public async Task<LotState> FindLotStateByLotIdAsync(int lotId)
        {
            return await context.LotStates.FirstOrDefaultAsync(x => x.LotId == lotId);
        }
        /// <summary>
        /// GEt all lotstate entity
        /// </summary
        public async Task<List<LotState>> GetAllLotStateAsync()
        {
            return await context.LotStates.ToListAsync();
        }
        /// <summary>
        /// Get user lot states by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<LotState>> GetUserLotstatesAsync(string userId)
        {
            return await context.LotStates.Where(x => x.OwnerId == userId).ToListAsync();
        }
        /// <summary>
        /// Update lotstate entity 
        /// </summary>
        /// <param name="lotState"></param>
        public void UpdateLotState(LotState lotState)
        {
            context.LotStates.Update(lotState);
        }
    }
}
