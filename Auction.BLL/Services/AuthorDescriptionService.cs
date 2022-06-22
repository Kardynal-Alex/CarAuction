using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace Auction.BLL.Services
{
    /// <summary>
    /// AuthorDescriptionService class
    /// </summary>
    public class AuthorDescriptionService : IAuthorDescriptionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AuthorDescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        /// <summary>
        /// Add AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        public async Task AddAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO)
        {
            ValidateAskOwnerModel(authorDescriptionDTO);

            await unitOfWork.AuthorDescriptionRepository.AddAuthorDescriptionAsync(
                mapper.Map<AuthorDescriptionDTO, AuthorDescription>(authorDescriptionDTO));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Update AuthorDescription
        /// </summary>
        /// <param name="authorDescription"></param>
        public async Task UpdateAuthorDescriptionAsync(AuthorDescriptionDTO authorDescriptionDTO)
        {
            ValidateAskOwnerModel(authorDescriptionDTO);

            unitOfWork.AuthorDescriptionRepository.UpdateAuthorDescription(
                  mapper.Map<AuthorDescriptionDTO, AuthorDescription>(authorDescriptionDTO));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Validate ask authorDescriptionDTO
        /// </summary>
        /// <param name="authorDescriptionDTO"></param>
        private void ValidateAskOwnerModel(AuthorDescriptionDTO authorDescriptionDTO)
        {
            Precognitions.StringIsNullOrEmpty(authorDescriptionDTO.UserId);
            Precognitions.StringIsNullOrEmpty(authorDescriptionDTO.Description);
            Precognitions.IntIsNotNumberOrNegative(authorDescriptionDTO.LotId, "Invalid number data");
        }
        /// <summary>
        /// Get Author Description ByLotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task<AuthorDescriptionDTO> GetAuthorDescriptionByLotIdAsync(int lotId)
        {
            Precognitions.IntIsNotNumberOrNegative(lotId);

            var authorDescription = await unitOfWork.AuthorDescriptionRepository.GetAuthorDescriptionByLotIdAsync(lotId);
            return mapper.Map<AuthorDescription, AuthorDescriptionDTO>(authorDescription);
        }
    }
}
