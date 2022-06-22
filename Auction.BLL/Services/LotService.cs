using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using DinkToPdf.Contracts;
using PDFGenerator.Models;
using PDFGenerator.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Services
{
    /// <summary>
    /// Lot Service class
    /// </summary>
    public class LotService : ILotService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConverter converter;
        public LotService(IUnitOfWork unitOfWork, IMapper mapper, IConverter converter)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.converter = converter;
        }
        /// <summary>
        /// Add lot and lotstate
        /// </summary>
        /// <param name="addLot"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task AddLotAsync(LotDTO addLot)
        {
            ValidateLotDTO(addLot);

            var lot = mapper.Map<LotDTO, Lot>(addLot);
            //+add images
            //+add lotstate
            await unitOfWork.LotRepository.AddLotAsync(lot);
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Delete lot and lotstate
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public async Task DeleteLotAsync(int lotId)
        {
            Precognitions.IntIsNotNumberOrNegative(lotId, "Invalid lot id");

            var lot = await unitOfWork.LotRepository.GetLotByIdAsync(lotId);
            var comments = await unitOfWork.CommentRepository.GetCommentsByLotIdAsync(lotId);
            //delete cascading with lotstate
            unitOfWork.LotRepository.DeleteLot(lot);
            unitOfWork.CommentRepository.DeleteCommentsRange(comments);

            unitOfWork.ImagesRepository.DeleteImagePhysicallyByPath(lot.Image);
            unitOfWork.ImagesRepository.DeleteImagesPhysically(lot.Images);
            unitOfWork.ImagesRepository.DeleteImagesById(lot.Images.Id);

            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Get all lots
        /// </summary>
        /// <returns>List of Lots</returns>
        public async Task<List<LotDTO>> GetAllLotsAsync()
        {
            return mapper.Map<List<LotDTO>>(await unitOfWork.LotRepository.GetAllLotsAsync());
        }
        /// <summary>
        /// Get favorite users lot by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<LotDTO>> GetFavoriteLotsByUserIdAsync(string userId)
        {
            Precognitions.StringIsNullOrEmpty(userId);

            var lots = await unitOfWork.LotRepository.GetFavoriteLotsByUserIdAsync(userId);
            return mapper.Map<List<LotDTO>>(lots);
        }
        /// <summary>
        /// Get fresh lot which are added today and yestarday
        /// </summary>
        /// <returns></returns>
        public async Task<List<LotDTO>> GetFreshLots()
        {
            var listFreshLot = await unitOfWork.LotRepository.GetFreshLots();
            return mapper.Map<List<LotDTO>>(listFreshLot);
        }
        /// <summary>
        /// Get lot by id with user details
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns>LotDTO</returns>
        public async Task<LotDTO> GetLotByIdWithDetailsAsync(int lotId)
        {
            Precognitions.IntIsNotNumberOrNegative(lotId, "Invalid lot id");

            var listlot = await unitOfWork.LotRepository.GetLotByIdWithDetailsAsync(lotId);
            return mapper.Map<Lot, LotDTO>(listlot);
        }
        /// <summary>
        /// Get lots by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of LotDTO</returns>
        public async Task<List<LotDTO>> GetLotsByUserIdAsync(string userId)
        {
            Precognitions.StringIsNullOrEmpty(userId);

            var listLot = await unitOfWork.LotRepository.GetLotsByUserIdAsync(userId);
            return mapper.Map<List<LotDTO>>(listLot);
        }
        /// <summary>
        /// Get Sold lots
        /// </summary>
        /// <returns></returns>
        public async Task<List<LotDTO>> GetSoldLotsAsync()
        {
            var lots = await unitOfWork.LotRepository.GetSoldLotsAsync();
            return mapper.Map<List<LotDTO>>(lots);
        }
        /// <summary>
        /// Get user bids(all lots where he made bid(can be sold lot and no yet))
        /// </summary>
        /// <param name="futureOwnerId"></param>
        /// <returns></returns>
        public async Task<List<LotDTO>> GetUserBidsAsync(string futureOwnerId)
        {
            var list = await unitOfWork.LotRepository.GetUserBidsAsync(futureOwnerId);
            return mapper.Map<List<LotDTO>>(list);
        }
        /// <summary>
        /// Update lot
        /// Close lot in user cabinet
        /// </summary>
        /// <param name="updateLot"></param>
        /// <returns></returns>
        public async Task UpdateLotAsync(LotDTO updateLot)
        {
            ValidateLotDTO(updateLot);

            if (updateLot.IsSold)
            {
                //send email if lot is sold
                var futureOwner = await unitOfWork.UserManager.FindByIdAsync(updateLot.LotState.FutureOwnerId);
                var createPDF = new CreatePDF(converter);
                var mappedLot = mapper.Map<LotDTO, Lot>(updateLot);
                var pdfFile = createPDF.CloseLotCreatePDF(mapper.Map<Lot, LotData>(mappedLot), mapper.Map<User, UserData>(futureOwner));
                var emailMessage = new EmailMessage
                {
                    To = futureOwner.Email,
                    Subject = "Cars & Bids",
                    Content = "You won an auction!",
                    PDFFile = pdfFile
                };
                await unitOfWork.EmailService.SendEmailAsync(emailMessage);
            }

            var lot = mapper.Map<LotDTO, Lot>(updateLot);
            unitOfWork.LotRepository.UpdateLot(lot);
            if (updateLot.LotState != null)
                unitOfWork.LotStateRepository.UpdateLotState(mapper.Map<LotStateDTO, LotState>(updateLot.LotState));
            if (updateLot.Images != null)
                unitOfWork.ImagesRepository.UpdateImages(mapper.Map<ImagesDTO, Images>(updateLot.Images));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Update lot after auto closing
        /// Close lot after timer is expired
        /// </summary>
        /// <param name="lotDTO"></param>
        /// <returns></returns>
        public async Task UpdateLotAfterClosingAsync(LotDTO updateLot)
        {
            ValidateLotDTO(updateLot);

            var lotState = await unitOfWork.LotStateRepository.FindLotStateByLotIdAsync(updateLot.Id);
            var futureOwner = await unitOfWork.UserManager.FindByIdAsync(lotState.FutureOwnerId);
            var owner = await unitOfWork.UserManager.FindByIdAsync(lotState.OwnerId);

            var lot = mapper.Map<LotDTO, Lot>(updateLot);
            lot.User = owner;
            //create pdf
            var createPDF = new CreatePDF(converter);
            var pdfFile = createPDF.CloseLotCreatePDF(mapper.Map<Lot, LotData>(lot), mapper.Map<User, UserData>(futureOwner));
            //form email
            var emailMessage = new EmailMessage
            {
                To = futureOwner.Email,
                Subject = "Cars & Bids",
                Content = "You won an auction!",
                PDFFile = pdfFile
            };
            await unitOfWork.EmailService.SendEmailAsync(emailMessage);

            unitOfWork.LotRepository.UpdateLot(lot);
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// validate lotDTO
        /// </summary>
        /// <param name="lotDTO"></param>
        private static void ValidateLotDTO(LotDTO lotDTO)
        {
            if (lotDTO.NameLot == null || lotDTO.Image == null || lotDTO.Description == null || lotDTO.UserId == null)
                throw new AuctionException("Invalid text data");

            if (lotDTO.NameLot.Length == 0 || lotDTO.Description.Length == 0 ||
                lotDTO.UserId.Length == 0 || lotDTO.Image.Length == 0)
                throw new AuctionException("Invalid length text data");

            if (!double.TryParse(lotDTO.StartPrice.ToString(), out double startPrice) ||
                !double.TryParse(lotDTO.CurrentPrice.ToString(), out double currentPrice) ||
                !double.TryParse(lotDTO.Year.ToString(), out double year))
                throw new AuctionException("Invalid number data");

            if (startPrice <= 0 || currentPrice <= 0 || year <= 0 || startPrice > currentPrice || year.ToString().Length != 4)  
                throw new AuctionException("Invalid number range data");
        }
        /// <summary>
        /// Update only date lot
        /// </summary>
        /// <param name="updateLot"></param>
        /// <returns></returns>
        public async Task UpdateOnlyDateLotAsync(LotDTO updateLot)
        {
            ValidateLotDTO(updateLot);

            updateLot.StartDateTime = DateTime.Now;
            unitOfWork.LotRepository.UpdateLot(mapper.Map<LotDTO, Lot>(updateLot));
            await unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Send email to owner
        /// </summary>
        /// <param name="askOwnerDTO"></param>
        /// <returns></returns>
        public async Task AskOwnerSendingEmailAsync(AskOwnerDTO askOwnerDTO)
        {
            ValidateAskOwnerModel(askOwnerDTO);

            var askOwner = mapper.Map<AskOwnerDTO, AskOwner>(askOwnerDTO);
            await unitOfWork.EmailService.SendEmailAsync(
                new EmailMessage
                {
                    To = askOwnerDTO.OwnerEmail,
                    Subject = "Questions from user Car & Bids",
                    Content = unitOfWork.EmailService.AskOwnerMessage(askOwner)
                });
        }
        /// <summary>
        /// Validate ask owner model
        /// </summary>
        /// <param name="askOwnerDTO"></param>
        private static void ValidateAskOwnerModel(AskOwnerDTO askOwnerDTO)
        {
            Precognitions.StringIsNullOrEmpty(askOwnerDTO.OwnerEmail);
            Precognitions.StringIsNullOrEmpty(askOwnerDTO.FullName);
            Precognitions.StringIsNullOrEmpty(askOwnerDTO.UserEmail);
            Precognitions.StringIsNullOrEmpty(askOwnerDTO.Text);
        }
    }
}
