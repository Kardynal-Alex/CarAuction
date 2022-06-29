using Auction.BLL.Services;
using Auction.BLL.Validation;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auction.Tests.BLLTests;
using Microsoft.Extensions.Options;
using DinkToPdf.Contracts;
using IdentityConstants = Auction.DAL.EF.IdentityConstants;
using Auction.BLL.DTO.Lot;
using Auction.BLL.DTO.Identity;

namespace Auction.Tests.BLLTests
{
    [TestFixture]
    public class LotServiceTests
    {
        private List<Lot> AllLots;
        private List<LotDTO> AllLotDTOs;
        [SetUp]
        public void Init()
        {
            using (var context = new ApplicationContext(UnitTestHelper.GetUnitDbOptions()))
            {
                AllLots = context.Lots.Include(x => x.User).Include(x => x.LotState).Include(x => x.Images).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Lot, LotDTO>();
                    cfg.CreateMap<User, UserDTO>();
                    cfg.CreateMap<LotState, LotStateDTO>();
                    cfg.CreateMap<Images, ImagesDTO>();
                });
                var mapper = new Mapper(config);
                AllLotDTOs = mapper.Map<List<LotDTO>>(AllLots);
            }
        }
        [Test]
        public async Task LotService_AddLot()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.AddLotAsync(It.IsAny<Lot>()));
            mockUnitOfWork.Setup(x => x.LotStateRepository.AddLotState(It.IsAny<LotState>()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = new LotDTO
            {
                Id = 7,
                NameLot = "2004 Mercedes-Benz SL63 AMG",
                StartPrice = 23000,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 23000,
                Year = 2004,
                Images = new ImagesDTO
                {
                    Id = 7,
                    Image1 = "Resources\\Images\\1-1.jpg",
                    Image2 = "Resources\\Images\\1-2.jpg",
                    Image3 = "Resources\\Images\\1-3.jpg",
                    Image4 = "Resources\\Images\\1-4.jpg",
                    Image5 = "Resources\\Images\\1-5.jpg",
                    Image6 = "Resources\\Images\\1-6.jpg",
                    Image7 = "Resources\\Images\\1-7.jpg",
                    Image8 = "Resources\\Images\\1-8.jpg",
                    Image9 = "Resources\\Images\\1-9.jpg"
                },
                LotState = new LotStateDTO
                {
                    Id = 7,
                    OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    FutureOwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    CountBid = 0,
                    LotId = 7
                }
            };

            await lotService.AddLotAsync(lotDTO);

            mockUnitOfWork.Verify(x => x.LotRepository.AddLotAsync(It.Is<Lot>(x =>
                  x.Id == lotDTO.Id && x.NameLot == lotDTO.NameLot && x.StartPrice == lotDTO.StartPrice &&
                  x.IsSold == lotDTO.IsSold && x.Image == lotDTO.Image && x.Description == lotDTO.Description &&
                  x.UserId == lotDTO.UserId && x.StartDateTime == lotDTO.StartDateTime &&
                  x.CurrentPrice == lotDTO.CurrentPrice && x.Year == lotDTO.Year &&
                  x.Images.Image1 == lotDTO.Images.Image1 && x.Images.Image2 == lotDTO.Images.Image2 &&
                  x.Images.Image3 == lotDTO.Images.Image3 && x.Images.Image4 == lotDTO.Images.Image4 &&
                  x.Images.Image5 == lotDTO.Images.Image5 && x.Images.Image6 == lotDTO.Images.Image6 &&
                  x.Images.Image7 == lotDTO.Images.Image7 && x.Images.Image8 == lotDTO.Images.Image8 &&
                  x.Images.Image9 == lotDTO.Images.Image9 && x.LotState.OwnerId == lotDTO.LotState.OwnerId &&
                  x.LotState.FutureOwnerId == lotDTO.LotState.FutureOwnerId && x.LotState.CountBid == lotDTO.LotState.CountBid &&
                  x.LotState.LotId == lotDTO.LotState.LotId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void LotService_AddLot_ThrowAuctionExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.AddLotAsync(It.IsAny<Lot>()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            //NameLot is empty
            var lotDTO = new LotDTO
            {
                Id = 7,
                NameLot = "",
                StartPrice = 23000,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 23000,
                Year = 2004
            };
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //Start price is negative number
            lotDTO.NameLot = "2004 Mercedes-Benz SL63 AMG";
            lotDTO.StartPrice = -23000;
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //Description is empty
            lotDTO.StartPrice = 23000;
            lotDTO.Description = "";
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //UserId is empty
            lotDTO.Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report";
            lotDTO.UserId = "";
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //Current Price is less than starting
            lotDTO.UserId = "925695ec-0e70-4e43-8514-8a0710e11d53";
            lotDTO.CurrentPrice = 21000;
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //Year is not correct
            lotDTO.CurrentPrice = 23000;
            lotDTO.Year = 123;
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));

            //Year is negative number
            lotDTO.Year = -1234;
            Assert.ThrowsAsync<AuctionException>(async () => await lotService.AddLotAsync(lotDTO));
        }

        [TestCase(1)]
        public async Task LotService_DeleteLot(int lotId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.DeleteLot(It.IsAny<Lot>()));
            mockUnitOfWork.Setup(x => x.CommentRepository.DeleteCommentsRange(It.IsAny<List<Comment>>()));
            mockUnitOfWork.Setup(x => x.ImagesRepository.DeleteImagesById(It.IsAny<int>()));

            var lot = AllLots.FirstOrDefault(x => x.Id == lotId);
            mockUnitOfWork.Setup(x => x.LotRepository.GetLotByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(lot));
            mockUnitOfWork.Setup(x => x.CommentRepository.GetCommentsByLotIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(CommentsByLotId()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            await lotService.DeleteLotAsync(lotId);

            mockUnitOfWork.Verify(x => x.LotRepository.DeleteLot(It.Is<Lot>(x => x.Id == lotId)), Times.Once);
            mockUnitOfWork.Verify(x => x.CommentRepository.DeleteCommentsRange(It.IsAny<List<Comment>>()), Times.Once());
            mockUnitOfWork.Verify(x => x.ImagesRepository.DeleteImagesById(It.IsAny<int>()), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        //lotId=1
        private static List<Comment> CommentsByLotId()
        {
            return new List<Comment>
            {
                new Comment
                {
                    Id = Guid.Parse("23f48b17-e073-f708-dcaa-5182ac94fd44"),
                    Author = "Ira Kardynal",
                    Text = "Bid $26000",
                    DateTime = DateTime.Parse("2021-06-15 11:33:27.8151988"),
                    LotId = 1,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    IsBid = true
                },
                new Comment
                {
                    Id = Guid.Parse("714c8742-ba41-12e4-2964-d216efc641c6"),
                    Author = "Oleksandr Kardynal",
                    Text = "Cool car!",
                    DateTime = DateTime.Parse("2021-06-15 11:08:42.1799046"),
                    LotId = 1,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    IsBid = false
                }
            };
        }

        [Test]
        public async Task LotService_GetAllLots()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetAllLotsAsync()).Returns(Task.FromResult(AllLots));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTOs = await lotService.GetAllLotsAsync();

            Assert.That(lotDTOs, Is.InstanceOf<List<LotDTO>>());
            Assert.AreEqual(lotDTOs.Count, AllLotDTOs.Count());
            Assert.That(lotDTOs, Is.EqualTo(AllLotDTOs).Using(new LotDTOEqualityComparer()));
        }

        [TestCase("5ae019a1-c312-4589-ab62-8b8a1fcb882c")]
        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotService_GetFavoriteLotsByUserId(string userId)
        {
            using var context = new ApplicationContext(UnitTestHelper.GetUnitDbOptions());
            var lots = AllLots.Where(x => context.Favorites.Any(y => y.UserId == userId && x.Id == y.LotId)).ToList();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetFavoriteLotsByUserIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(lots.ToList()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTOs = await lotService.GetFavoriteLotsByUserIdAsync(userId);

            Assert.That(lotDTOs, Is.InstanceOf<List<LotDTO>>());
            Assert.AreEqual(lotDTOs.Count, lots.Count());
            for (int i = 0; i < lotDTOs.Count(); i++)
            {
                Assert.AreEqual(lotDTOs[i].Id, lots[i].Id);
                Assert.AreEqual(lotDTOs[i].UserId, lots[i].UserId);
                Assert.AreEqual(lotDTOs[i].NameLot, lots[i].NameLot);
            }
        }

        [TestCase(1)]
        public async Task LotService_GetLotByIdWithDetails(int lotId)
        {
            var lots = AllLots.First(x => x.Id == lotId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetLotByIdWithDetailsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(lots));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = await lotService.GetLotByIdWithDetailsAsync(lotId);
            var expected = AllLotDTOs.FirstOrDefault(x => x.Id == lotId);

            Assert.IsInstanceOf<LotDTO>(lotDTO);
            Assert.That(lotDTO, Is.EqualTo(expected).Using(new LotDTOEqualityComparer()));
            Assert.That(lotDTO.User, Is.EqualTo(expected.User).Using(new UserDTOEqualityComparer()));
            Assert.That(lotDTO.LotState, Is.EqualTo(expected.LotState).Using(new LotStateDTOEqualityComparer()));
            Assert.That(lotDTO.Images, Is.EqualTo(expected.Images).Using(new ImagesDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotService_GetLotsByUserIdAsync(string userId)
        {
            var lots = AllLots.Where(x => x.UserId == userId).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetLotsByUserIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(lots));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTOs = await lotService.GetLotsByUserIdAsync(userId);
            var expected = AllLotDTOs.Where(x => x.UserId == userId).ToList();

            Assert.IsInstanceOf<List<LotDTO>>(lotDTOs);
            Assert.AreEqual(lotDTOs.Count, lots.Count());
            Assert.That(lotDTOs, Is.EqualTo(expected).Using(new LotDTOEqualityComparer()));
        }

        [Test]
        public async Task LotService_GetSoldLots()
        {
            var lots = AllLots.Where(x => x.IsSold == true).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetSoldLotsAsync())
                .Returns(Task.FromResult(lots));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var soldLotDTOs = await lotService.GetSoldLotsAsync();
            var expected = AllLotDTOs.Where(x => x.IsSold == true);

            Assert.IsInstanceOf<List<LotDTO>>(soldLotDTOs);
            Assert.AreEqual(soldLotDTOs.Count, lots.Count());
            Assert.That(soldLotDTOs, Is.EqualTo(expected).Using(new LotDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotService_GetUserBids(string fututreOwnerId)
        {
            var ExpectedLots = AllLots.Where(x => x.UserId != fututreOwnerId && x.LotState.FutureOwnerId == fututreOwnerId)
                .OrderBy(x => x.Id).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.GetUserBidsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(ExpectedLots));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lots = await lotService.GetUserBidsAsync(fututreOwnerId);
            Assert.IsInstanceOf<List<LotDTO>>(lots);
            Assert.AreEqual(lots.Count, ExpectedUserBids().Count);
            Assert.That(lots, Is.EqualTo(ExpectedUserBids())
                .Using(new LotDTOEqualityComparer()));
            Assert.That(lots.Select(x => x.User), Is.EqualTo(ExpectedUserBids().Select(x => x.User))
                .Using(new UserDTOEqualityComparer()));
            Assert.That(lots.Select(x => x.LotState), Is.EqualTo(ExpectedUserBids().Select(x => x.LotState))
                .Using(new LotStateDTOEqualityComparer()));
        }

        private static List<LotDTO> ExpectedUserBids()
        {
            return new List<LotDTO>
            {
                new LotDTO
                {
                    Id = 4,
                    NameLot = "2010 Nissan 370Z Roadster",
                    StartPrice = 11200,
                    IsSold = true,
                    Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-(edit).jpg",
                    Description = "~54,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report",
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    StartDateTime = DateTime.Parse("2021-06-15 07:38:27.6820000"),
                    CurrentPrice = 11200,
                    Year = 2010,
                    User = new UserDTO
                    {
                        Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                        Name = "Ira",
                        Surname = "Kardynal",
                        Role = IdentityConstants.User,
                        Email = "irakardinal@gmail.com"
                    },
                    LotState = new LotStateDTO
                    {
                        Id = 4,
                        OwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                        FutureOwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                        LotId = 4,
                        CountBid = 3
                    }
                }
            };
        }

        [Test]
        public async Task LotService_UpdateLotIfLotStateIsNullAndImagesIsNull()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.UpdateLot(It.IsAny<Lot>()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = new LotDTO
            {
                Id = 7,
                NameLot = "2004 Mercedes-Benz SL63 AMG",
                StartPrice = 23000,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 23000,
                Year = 2004,
                LotState = null,
                Images = null
            };

            await lotService.UpdateLotAsync(lotDTO);

            mockUnitOfWork.Verify(x => x.LotRepository.UpdateLot(It.Is<Lot>(x =>
                  x.Id == lotDTO.Id && x.NameLot == lotDTO.NameLot && x.StartPrice == lotDTO.StartPrice &&
                  x.IsSold == lotDTO.IsSold && x.Image == lotDTO.Image && x.Description == lotDTO.Description &&
                  x.UserId == lotDTO.UserId && x.StartDateTime == lotDTO.StartDateTime &&
                  x.CurrentPrice == lotDTO.CurrentPrice && x.Year == lotDTO.Year)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.AtLeastOnce);
        }

        [Test]
        public async Task LotService_UpdateLotIfLotStateIsNotNullAndImagesIsNotNull()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.UpdateLot(It.IsAny<Lot>()));
            mockUnitOfWork.Setup(x => x.LotStateRepository.UpdateLotState(It.IsAny<LotState>()));
            mockUnitOfWork.Setup(x => x.ImagesRepository.UpdateImages(It.IsAny<Images>()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = new LotDTO
            {
                Id = 1,
                NameLot = "2004 Mercedes-Benz SL63 AMG",
                StartPrice = 23000,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 23000,
                Year = 2004,
                LotState = new LotStateDTO
                {
                    Id = 1,
                    OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    LotId = 1,
                    CountBid = 1
                },
                Images = new ImagesDTO
                {
                    Id = 1,
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg",
                    Image5 = "Resources\\Images\\11-5.jpg",
                    Image6 = "Resources\\Images\\11-6.jpg",
                    Image7 = "Resources\\Images\\11-7.jpg",
                    Image8 = "Resources\\Images\\11-8.jpg",
                    Image9 = "Resources\\Images\\11-9.jpg"
                }
            };

            await lotService.UpdateLotAsync(lotDTO);

            mockUnitOfWork.Verify(x => x.LotRepository.UpdateLot(It.Is<Lot>(x =>
                  x.Id == lotDTO.Id && x.NameLot == lotDTO.NameLot && x.StartPrice == lotDTO.StartPrice &&
                  x.IsSold == lotDTO.IsSold && x.Image == lotDTO.Image && x.Description == lotDTO.Description &&
                  x.UserId == lotDTO.UserId && x.StartDateTime == lotDTO.StartDateTime &&
                  x.CurrentPrice == lotDTO.CurrentPrice && x.Year == lotDTO.Year
                 )), Times.Once);

            mockUnitOfWork.Verify(x => x.ImagesRepository.UpdateImages(It.Is<Images>(x =>
                   x.Image1 == lotDTO.Images.Image1 && x.Image2 == lotDTO.Images.Image2 &&
                   x.Image3 == lotDTO.Images.Image3 && x.Image4 == lotDTO.Images.Image4 &&
                   x.Image5 == lotDTO.Images.Image5 && x.Image6 == lotDTO.Images.Image6 &&
                   x.Image7 == lotDTO.Images.Image7 && x.Image8 == lotDTO.Images.Image8 &&
                   x.Image9 == lotDTO.Images.Image9)), Times.Once);

            mockUnitOfWork.Verify(x => x.LotStateRepository.UpdateLotState(It.Is<LotState>(x =>
                x.Id == lotDTO.LotState.Id && x.OwnerId == lotDTO.LotState.OwnerId &&
                x.FutureOwnerId == lotDTO.LotState.FutureOwnerId && x.CountBid == lotDTO.LotState.CountBid &&
                x.LotId == lotDTO.LotState.LotId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.AtLeastOnce);
        }

        [Test]
        public async Task LotService_UpdateOnlyDateLotToCurrentDate()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.LotRepository.UpdateLot(It.IsAny<Lot>()));
            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = new LotDTO
            {
                Id = 1,
                NameLot = "2004 Mercedes-Benz SL63 AMG",
                StartPrice = 23000,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 23000,
                Year = 2004
            };

            await lotService.UpdateLotAsync(lotDTO);

            mockUnitOfWork.Verify(x => x.LotRepository.UpdateLot(It.Is<Lot>(x =>
                  x.Id == lotDTO.Id && x.NameLot == lotDTO.NameLot && x.StartPrice == lotDTO.StartPrice &&
                  x.IsSold == lotDTO.IsSold && x.Image == lotDTO.Image && x.Description == lotDTO.Description &&
                  x.UserId == lotDTO.UserId && x.StartDateTime == lotDTO.StartDateTime &&
                  x.CurrentPrice == lotDTO.CurrentPrice && x.Year == lotDTO.Year
                 )), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.AtLeastOnce);
        }

        [Test]
        public async Task LotService_UpdateLot_CloseLot()
        {
            var user = new User
            {
                Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                Name = "Ira",
                UserName = "irakardinal@gmail.com",
                Surname = "Kardynal",
                Role = IdentityConstants.User,
                Email = "irakardinal@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
            };

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            mockUserStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(user);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockConverter = new Mock<IConverter>();
            mockUnitOfWork.Setup(x => x.UserManager).Returns(userManager);
            mockUnitOfWork.Setup(x => x.LotRepository.UpdateLot(It.IsAny<Lot>()));
            mockUnitOfWork.Setup(x => x.LotStateRepository.UpdateLotState(It.IsAny<LotState>()));
            mockUnitOfWork.Setup(x => x.EmailService.SendEmailAsync(It.IsAny<EmailMessage>()));

            var lotService = new LotService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL(), mockConverter.Object);

            var lotDTO = new LotDTO
            {
                Id = 1,
                NameLot = "2004 Mercedes-Benz SL63 AMG",
                StartPrice = 23000,
                IsSold = true,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0d.jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-20 07:33:48.0630000"),
                CurrentPrice = 24000,
                Year = 2004,
                LotState = new LotStateDTO
                {
                    Id = 1,
                    OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    LotId = 1,
                    CountBid = 1
                },
                User = new UserDTO
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    Surname = "Kardynal",
                    Role = IdentityConstants.Admin,
                    Email = "admin@gmail.com"
                }
            };

            await lotService.UpdateLotAsync(lotDTO);

            mockUnitOfWork.Verify(x => x.LotRepository.UpdateLot(It.Is<Lot>(x =>
                  x.Id == lotDTO.Id && x.NameLot == lotDTO.NameLot && x.StartPrice == lotDTO.StartPrice &&
                  x.IsSold == lotDTO.IsSold && x.Image == lotDTO.Image && x.Description == lotDTO.Description &&
                  x.UserId == lotDTO.UserId && x.StartDateTime == lotDTO.StartDateTime &&
                  x.CurrentPrice == lotDTO.CurrentPrice && x.Year == lotDTO.Year
                 )), Times.Once);

            mockUnitOfWork.Verify(x => x.LotStateRepository.UpdateLotState(It.Is<LotState>(x =>
                x.Id == lotDTO.LotState.Id && x.OwnerId == lotDTO.LotState.OwnerId &&
                x.FutureOwnerId == lotDTO.LotState.FutureOwnerId && x.CountBid == lotDTO.LotState.CountBid &&
                x.LotId == lotDTO.LotState.LotId)), Times.Once);

            mockUnitOfWork.Verify(x => x.EmailService.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.AtLeastOnce);
        }

        [Test]
        public async Task LotService_AddAuthorDescription()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.AuthorDescriptionRepository.AddAuthorDescriptionAsync(It.IsAny<AuthorDescription>()));
            var authorDescriptionService = new AuthorDescriptionService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var authorDescriptionDTO = new AuthorDescriptionDTO
            {
                Id = 3,
                LotId = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Description = "Description 3"
            };

            await authorDescriptionService.AddAuthorDescriptionAsync(authorDescriptionDTO);

            mockUnitOfWork.Verify(x => x.AuthorDescriptionRepository.AddAuthorDescriptionAsync(It.Is<AuthorDescription>(x =>
                 x.Id == authorDescriptionDTO.Id && x.LotId == authorDescriptionDTO.LotId &&
                 x.Description == authorDescriptionDTO.Description && x.UserId == authorDescriptionDTO.UserId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void LotService_AddAuthorDescription_ThrowAuctionExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.AuthorDescriptionRepository.AddAuthorDescriptionAsync(It.IsAny<AuthorDescription>()));
            var authorDescriptionService = new AuthorDescriptionService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            //LotId is negative
            var authorDescriptionDTO = new AuthorDescriptionDTO
            {
                Id = 3,
                LotId = -1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Description = "Description 3"
            };
            Assert.ThrowsAsync<AuctionException>(async () => await authorDescriptionService.AddAuthorDescriptionAsync(authorDescriptionDTO));

            //UserId empty
            authorDescriptionDTO.LotId = 1;
            authorDescriptionDTO.UserId = "";
            Assert.ThrowsAsync<AuctionException>(async () => await authorDescriptionService.AddAuthorDescriptionAsync(authorDescriptionDTO));

            //Description empty
            authorDescriptionDTO.Description = "";
            authorDescriptionDTO.UserId = "925695ec-0e70-4e43-8514-8a0710e11d53";
            Assert.ThrowsAsync<AuctionException>(async () => await authorDescriptionService.AddAuthorDescriptionAsync(authorDescriptionDTO));
        }

        [TestCase(1)]
        public async Task LotService_GetAuthorDescriptionByLotId(int lotId)
        {
            var authorDescription = ExpectedAuthorDTODescriptions.First(x => x.LotId == lotId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.AuthorDescriptionRepository.GetAuthorDescriptionByLotIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(
                    new AuthorDescription
                    {
                        Id = 1,
                        LotId = 1,
                        UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                        Description = "Description 1"
                    }));
            var authorDescriptionService = new AuthorDescriptionService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var authorDescriptionDTO = await authorDescriptionService.GetAuthorDescriptionByLotIdAsync(lotId);
            var expected = ExpectedAuthorDTODescriptions.FirstOrDefault(x => x.LotId == lotId);

            Assert.IsInstanceOf<AuthorDescriptionDTO>(authorDescriptionDTO);
            Assert.That(authorDescriptionDTO, Is.EqualTo(expected).Using(new AuthorDescriptionDTOEqualityComparer()));
        }

        private static IEnumerable<AuthorDescriptionDTO> ExpectedAuthorDTODescriptions =>
            new[]
            {
                new AuthorDescriptionDTO
                {
                    Id = 1,
                    LotId = 1,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Description = "Description 1"
                },
                new AuthorDescriptionDTO
                {
                    Id = 2,
                    LotId = 2,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Description = "Description 2"
                }
            };
    }
}
