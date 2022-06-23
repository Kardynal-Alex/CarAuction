using Auction.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface ILotStateRepository
    {
        void AddLotState(LotState addLotState);
        Task DeleteLotStateByLotIdAsync(int lotId);
        void UpdateLotState(LotState lotState);
        Task<LotState> FindLotStateByLotIdAsync(int lotId);
        Task<List<LotState>> GetAllLotStateAsync();
        void DeleteLotStatesRange(List<LotState> lotStates);
        Task<List<LotState>> GetUserLotstatesAsync(string userId);
    }
}
