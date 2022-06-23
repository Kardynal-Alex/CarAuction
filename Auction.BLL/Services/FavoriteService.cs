using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
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
      
        public async Task AddFavoriteAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            await unitOfWork.FavoriteRepository.AddFavoriteAsync(mapper.Map<FavoriteDTO, Favorite>(favoriteDTO));
            await unitOfWork.SaveAsync();
        }
     
        public async Task DeleteFavoriteByLotIdAndUserIdAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            await unitOfWork.FavoriteRepository.DeleteByLotIdAndUserIdAsync(mapper.Map<FavoriteDTO, Favorite>(favoriteDTO));
            await unitOfWork.SaveAsync();
        }
      
        public async Task DeleteFavoriteAsync(string id)
        {
            Precognitions.StringIsNullOrEmpty(id);

            unitOfWork.FavoriteRepository.DeleteFavorite(id);
            await unitOfWork.SaveAsync();
        }
      
        public async Task<List<FavoriteDTO>> GetFavoriteByUserIdAsync(string userId)
        {
            Precognitions.StringIsNullOrEmpty(userId, "Icorect userId");

            var favorites = await unitOfWork.FavoriteRepository.GetFavoriteByUserIdAsync(userId);
            return mapper.Map<List<FavoriteDTO>>(favorites);
        }
      
        private static void ValidateFavoriteDTO(FavoriteDTO favoriteDTO)
        {
            Precognitions.IntIsNotNumberOrNegative(favoriteDTO.LotId);
            Precognitions.StringIsNullOrEmpty(favoriteDTO.UserId, "Incorrect length");
        }
      
        public async Task DeleteInRangeFavoritesAsync(List<FavoriteDTO> favoriteDTOs)
        {
            var favorites = mapper.Map<List<Favorite>>(favoriteDTOs);
            unitOfWork.FavoriteRepository.DeleteInRangeFavorites(favorites);
            await unitOfWork.SaveAsync();
        }
       
        public async Task<FavoriteDTO> GetFavoriteByUserIdAndLotIdAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTO(favoriteDTO);

            var mappedFavorite = mapper.Map<FavoriteDTO, Favorite>(favoriteDTO);
            var favorite = await unitOfWork.FavoriteRepository.GetFavoriteByUserIdAndLotIdAsync(mappedFavorite);
            return mapper.Map<Favorite, FavoriteDTO>(favorite);
        }
    }
}
