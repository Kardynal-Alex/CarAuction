using Auction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// ILotRepository
    /// </summary>
    public interface ILotRepository
    {
        /// <summary>
        /// Add lot Entity
        /// </summary>
        Task AddLotAsync(Lot addLot);
        /// <summary>
        /// Delete lot state Entity
        /// </summary>
        void DeleteLot(Lot lot);
        /// <summary>
        /// Get lot by id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public Task<Lot> GetLotByIdAsync(int lotId);
        /// <summary>
        /// Update lot state Entity
        /// </summary>
        void UpdateLot(Lot updateLot);
        /// <summary>
        /// get lots Entity by user id
        /// </summary>
        Task<List<Lot>> GetLotsByUserIdAsync(string userId);
        /// <summary>
        /// Get all lot Entity
        /// </summary>
        Task<List<Lot>> GetAllLotsAsync();
        /// <summary>
        /// Get lot Entity with user detail by lotid
        /// </summary>
        Task<Lot> GetLotByIdWithDetailsAsync(int lotId);
        /// <summary>
        /// Get lot user bids Entity(when he made bid for lot)
        /// </summary>
        Task<List<Lot>> GetUserBidsAsync(string futureOwnerId);
        /// <summary>
        /// Delete users lot
        /// </summary>
        /// <param name="lots"></param>
        void DeleteLotsRange(List<Lot> lots);
        /// <summary>
        /// Delete lot by lot id
        /// </summary>
        /// <param name="lotId"></param>
        void DeleteLotByLotId(int lotId);
        /// <summary>
        /// Get fresh lot which are added today and yestarday
        /// </summary>
        /// <returns></returns>
        Task<List<Lot>> GetFreshLots();
        /// <summary>
        /// Get favorite lots by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Lot>> GetFavoriteLotsByUserIdAsync(string userId);
        /// <summary>
        /// Get sold lot
        /// </summary>
        /// <returns></returns>
        Task<List<Lot>> GetSoldLotsAsync();
        /// <summary>
        /// Add AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        Task AddAuthorDescriptionAsync(AuthorDescription authorDescription);
        /// <summary>
        /// Update AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        void UpdateAuthorDescription(AuthorDescription authorDescription);
        /// <summary>
        /// Get Author Description ByLotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        Task<AuthorDescription> GetAuthorDescriptionByLotIdAsync(int lotId);
    }
}
