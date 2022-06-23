using Auction.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface ILotRepository
    {
        Task AddLotAsync(Lot addLot);
        void DeleteLot(Lot lot);
        public Task<Lot> GetLotByIdAsync(int lotId);
        void UpdateLot(Lot updateLot);
        Task<List<Lot>> GetLotsByUserIdAsync(string userId);
        Task<List<Lot>> GetAllLotsAsync();
        Task<Lot> GetLotByIdWithDetailsAsync(int lotId);
        Task<List<Lot>> GetUserBidsAsync(string futureOwnerId);
        void DeleteLotsRange(List<Lot> lots);
        void DeleteLotByLotId(int lotId);
        Task<List<Lot>> GetFreshLots();
        Task<List<Lot>> GetFavoriteLotsByUserIdAsync(string userId);
        Task<List<Lot>> GetSoldLotsAsync();
    }
}
