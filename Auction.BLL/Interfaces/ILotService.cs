using Auction.BLL.DTO.FilterModels;
using Auction.BLL.DTO.Lot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    public interface ILotService
    {
        Task AddLotAsync(LotDTO addLot);
        Task DeleteLotAsync(int lotId);
        Task UpdateLotAsync(LotDTO updateLot);
        Task<List<LotDTO>> GetLotsByUserIdAsync(string userId);
        Task<List<LotDTO>> GetAllLotsAsync();
        Task<LotDTO> GetLotByIdWithDetailsAsync(int lotId);
        Task<List<LotDTO>> GetUserBidsAsync(string futureOwnerId);
        Task<List<LotDTO>> GetFreshLots();
        Task<List<LotDTO>> GetFavoriteLotsByUserIdAsync(string userId);
        Task<List<LotDTO>> GetSoldLotsAsync();
        Task UpdateLotAfterClosingAsync(LotDTO updateLot);
        Task UpdateOnlyDateLotAsync(LotDTO updateLot);
        Task AskOwnerSendingEmailAsync(AskOwnerDTO askOwnerDTO);
        Task<List<LotDTO>> FetchFilteredAsync(PageRequest pageRequest);
    }
}
