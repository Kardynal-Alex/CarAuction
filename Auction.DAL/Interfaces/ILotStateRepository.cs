using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// ILotStateRepository
    /// </summary>
    public interface ILotStateRepository
    {
        /// <summary>
        /// Add lot state Entity
        /// </summary>
        void AddLotState(LotState addLotState);
        /// <summary>
        /// Delete lot state Entity by id
        /// </summary>
        Task DeleteLotStateByLotIdAsync(int lotId);
        /// <summary>
        /// Update lot state Entity
        /// </summary>
        void UpdateLotState(LotState lotState);
        /// <summary>
        /// Find lot state Entity by lotId
        /// </summary>
        Task<LotState> FindLotStateByLotIdAsync(int lotId);
        /// <summary>
        /// Get all lot state Entity
        /// </summary>
        Task<List<LotState>> GetAllLotStateAsync();
        /// <summary>
        /// Delete user lotstates 
        /// </summary>
        /// <param name="lotStates"></param>
        void DeleteLotStatesRange(List<LotState> lotStates);
        /// <summary>
        /// Get user lotstates
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<LotState>> GetUserLotstatesAsync(string userId);
    }
}
