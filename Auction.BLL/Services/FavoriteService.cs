using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        /// <summary>
        /// Add favorite
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        public async Task AddFavoriteAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            await unitOfWork.FavoriteRepository.AddFavoriteAsync(mapper.Map<FavoriteDTO, Favorite>(favoriteDTO));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Delete favorite by lot id and user id
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        public async Task DeleteFavoriteByLotIdAndUserIdAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            await unitOfWork.FavoriteRepository.DeleteByLotIdAndUserIdAsync(mapper.Map<FavoriteDTO, Favorite>(favoriteDTO));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Delete favorite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteFavoriteAsync(string id)
        {
            if (id.Length == 0)   
                throw new AuctionException("incorect id");

            unitOfWork.FavoriteRepository.DeleteFavorite(id);
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// get user favorite lots by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FavoriteDTO>> GetFavoriteByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) 
                throw new AuctionException("Icorect userId");

            var favorites = await unitOfWork.FavoriteRepository.GetFavoriteByUserIdAsync(userId);
            return mapper.Map<List<FavoriteDTO>>(favorites);
        }
        /// <summary>
        /// Validate input data for favoriteDTO
        /// </summary>
        /// <param name="favoriteDTO"></param>
        private void ValidateFavoriteDTO(FavoriteDTO favoriteDTO)
        {
            if (favoriteDTO.UserId == null || !int.TryParse(favoriteDTO.LotId.ToString(), out int fav))  
                throw new AuctionException("Incorrect data");

            if (favoriteDTO.UserId == "" || fav < 0)
                throw new AuctionException("Incorrect data");

            if (favoriteDTO.UserId.Length == 0)
                throw new AuctionException("Incorrect length");
        }
        /// <summary>
        /// Delete user favorite lots
        /// </summary>
        /// <param name="favoriteDTOs"></param>
        /// <returns></returns>
        public async Task DeleteInRangeFavoritesAsync(List<FavoriteDTO> favoriteDTOs)
        {
            var favorites = mapper.Map<List<Favorite>>(favoriteDTOs);
            unitOfWork.FavoriteRepository.DeleteInRangeFavorites(favorites);
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Get favorite by user id and lot id
        /// </summary>
        /// <param name="favoriteDTO"></param>
        /// <returns></returns>
        public async Task<FavoriteDTO> GetFavoriteByUserIdAndLotIdAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            var mappedFavorite = mapper.Map<FavoriteDTO, Favorite>(favoriteDTO);
            var favorite = await unitOfWork.FavoriteRepository.GetFavoriteByUserIdAndLotIdAsync(mappedFavorite);
            return mapper.Map<Favorite, FavoriteDTO>(favorite);
        }
    }
}
