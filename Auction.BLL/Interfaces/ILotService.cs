using Auction.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// ILotService
    /// </summary>
    public interface ILotService
    {
        /// <summary>
        /// Add lot
        /// </summary>
        /// <param name="addLot"></param>
        /// <returns></returns>
        Task AddLotAsync(LotDTO addLot);
        /// <summary>
        /// Delete lot by lotid
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task DeleteLotAsync(int lotId);
        /// <summary>
        /// Update lot
        /// </summary>
        /// <param name="updateLot"></param>
        /// <returns></returns>
        Task UpdateLotAsync(LotDTO updateLot);
        /// <summary>
        /// Get lots by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of lots</returns>
        Task<List<LotDTO>> GetLotsByUserIdAsync(string userId);
        /// <summary>
        /// Get all lots
        /// </summary>
        /// <returns>List of all lots</returns>
        Task<List<LotDTO>> GetAllLotsAsync();
        /// <summary>
        /// Get lot by id with user details
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns>Lot</returns>
        Task<LotDTO> GetLotByIdWithDetailsAsync(int lotId);
        /// <summary>
        /// Get user bids(all lots where he made bid(can be sold lot and no yet))
        /// </summary>
        /// <param name="futureOwnerId"></param>
        /// <returns>List of lots</returns>
        Task<List<LotDTO>> GetUserBidsAsync(string futureOwnerId);
        /// <summary>
        /// Get fresh lot which are added today and yestarday
        /// </summary>
        /// <returns></returns>
        Task<List<LotDTO>> GetFreshLots();
        /// <summary>
        /// Get favorite users lot by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<LotDTO>> GetFavoriteLotsByUserIdAsync(string userId);
        /// <summary>
        /// Get sold lots
        /// </summary>
        /// <returns></returns>
        Task<List<LotDTO>> GetSoldLotsAsync();
        /// <summary>
        /// UpdateLot after automativcalyy closing
        /// </summary>
        /// <param name="lotDTO"></param>
        /// <returns></returns>
        Task UpdateLotAfterClosingAsync(LotDTO updateLot);
        /// <summary>
        /// Update only date lot
        /// </summary>
        /// <param name="updateLot"></param>
        /// <returns></returns>
        Task UpdateOnlyDateLotAsync(LotDTO updateLot);
        /// <summary>
        /// Ask owner by sending him email
        /// </summary>
        /// <param name="askOwnerDTO"></param>
        /// <returns></returns>
        Task AskOwnerSendingEmailAsync(AskOwnerDTO askOwnerDTO);
    }
}
