using Auction.BLL.DTO.Lot;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace Auction.BLL.Services
{
    public class AuthorDescriptionService : IAuthorDescriptionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AuthorDescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        public async Task AddAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO)
        {
            ValidateAskOwnerModel(authorDescriptionDTO);

            await unitOfWork.AuthorDescriptionRepository.AddAuthorDescriptionAsync(
                mapper.Map<AuthorDescriptionDTO, AuthorDescription>(authorDescriptionDTO));
            await unitOfWork.SaveAsync();
        }
        
        public async Task UpdateAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO)
        {
            ValidateAskOwnerModel(authorDescriptionDTO);

            unitOfWork.AuthorDescriptionRepository.UpdateAuthorDescription(
                  mapper.Map<AuthorDescriptionDTO, AuthorDescription>(authorDescriptionDTO));
            await unitOfWork.SaveAsync();
        }
       
        private void ValidateAskOwnerModel(AuthorDescriptionDTO authorDescriptionDTO)
        {
            Precognitions.StringIsNullOrEmpty(authorDescriptionDTO.UserId);
            Precognitions.StringIsNullOrEmpty(authorDescriptionDTO.Description);
            Precognitions.IntIsNotNumberOrNegative(authorDescriptionDTO.LotId, "Invalid number data");
        }
       
        public async Task<AuthorDescriptionDTO> GetAuthorDescriptionByLotIdAsync(int lotId)
        {
            Precognitions.IntIsNotNumberOrNegative(lotId);

            var authorDescription = await unitOfWork.AuthorDescriptionRepository.GetAuthorDescriptionByLotIdAsync(lotId);
            return mapper.Map<AuthorDescription, AuthorDescriptionDTO>(authorDescription);
        }
    }
}
