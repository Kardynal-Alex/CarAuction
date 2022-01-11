using Auction.BLL.DTO;
using Auction.BLL.Services;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Tests.BLLTests
{
    [TestFixture]
    public class FavoriteServiceTests
    {
        [Test]
        public async Task FavoriteService_AddFavorite()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.AddFavoriteAsync(It.IsAny<Favorite>()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var favoriteDTO = new FavoriteDTO { Id = new Guid().ToString(), UserId = "0bbbf130-60c7-4f80-b1ea-85ba6c4d0a8c", LotId = 1 };
            await favoriteService.AddFavoriteAsync(favoriteDTO);

            mockUnitOfWork.Verify(x => x.FavoriteRepository.AddFavoriteAsync(It.Is<Favorite>(x =>
                  x.Id == favoriteDTO.Id && x.UserId == favoriteDTO.UserId && x.LotId == favoriteDTO.LotId)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void FavoriteService_AddFavorite_ThrowAuctionExceptionIfModelIsIncorect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.AddFavoriteAsync(It.IsAny<Favorite>()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            //UserId is empty
            var favoriteDTO = new FavoriteDTO { Id = new Guid().ToString(), UserId = "", LotId = 1 };
            Assert.ThrowsAsync<AuctionException>(async () => await favoriteService.AddFavoriteAsync(favoriteDTO));

            //LotId is negative number
            favoriteDTO.LotId = -1;
            favoriteDTO.UserId = "0bbbf130-60c7-4f80-b1ea-85ba6c4d0a8c";
            Assert.ThrowsAsync<AuctionException>(async () => await favoriteService.AddFavoriteAsync(favoriteDTO));
        }

        [Test]
        public async Task FavoriteService_DeleteFavoriteByLotIdAndUserId()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.DeleteByLotIdAndUserIdAsync(It.IsAny<Favorite>()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var favorite = new Favorite { LotId = 1, UserId = "925695ec-0e70-4e43-8514-8a0710e11d53" };
            var favoriteDTO = new FavoriteDTO { LotId = 1, UserId = "925695ec-0e70-4e43-8514-8a0710e11d53" };

            await favoriteService.DeleteFavoriteByLotIdAndUserIdAsync(favoriteDTO);

            mockUnitOfWork.Verify(x => x.FavoriteRepository.DeleteByLotIdAndUserIdAsync(It.Is<Favorite>(x =>
                x.LotId == favorite.LotId && x.UserId == favorite.UserId)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task FavoriteService_DeleteFavoriteById(string id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.DeleteFavorite(It.IsAny<string>()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            await favoriteService.DeleteFavoriteAsync(id);

            mockUnitOfWork.Verify(x => x.FavoriteRepository.DeleteFavorite(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FavoriteService_GetFavoritesByUserId(string userId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.GetFavoriteByUserIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Favorites()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var favoriteDTOs = await favoriteService.GetFavoriteByUserIdAsync(userId);
            var expected = Favorites();

            Assert.IsInstanceOf<List<FavoriteDTO>>(favoriteDTOs);
            Assert.That(favoriteDTOs, Is.EqualTo(FavoriteDTOs()).Using(new FavoriteDTOEqualityComparer()));
        }

        private static List<Favorite> Favorites()
        {
            return new List<Favorite>
            {
                new Favorite{ Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 1 },
                new Favorite{ Id = "cccccccc-cccc-cccc-cccc-cccccccccccc", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 }
            };
        }

        private static List<FavoriteDTO> FavoriteDTOs()
        {
            return new List<FavoriteDTO>
            {
                new FavoriteDTO { Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 1 },
                new FavoriteDTO { Id = "cccccccc-cccc-cccc-cccc-cccccccccccc", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 }
            };
        }

        [Test]
        public async Task FavoriteService_DeleteFavoritesInRange()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.DeleteInRangeFavorites(It.IsAny<List<Favorite>>()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            await favoriteService.DeleteInRangeFavoritesAsync(FavoriteDTOs());

            mockUnitOfWork.Verify(x => x.FavoriteRepository.DeleteInRangeFavorites(It.IsAny<List<Favorite>>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task FavoriteService_GetFavoritesByUserIdAndLotId()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.GetFavoriteByUserIdAndLotIdAsync(It.IsAny<Favorite>()))
                .Returns(Task.FromResult(Favorites().First()));
            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var newFavorite = new FavoriteDTO { LotId = 1, UserId = "925695ec-0e70-4e43-8514-8a0710e11d53" };
            var favoriteDTO = await favoriteService.GetFavoriteByUserIdAndLotIdAsync(newFavorite);

            Assert.IsInstanceOf<FavoriteDTO>(favoriteDTO);
            Assert.That(favoriteDTO, Is.EqualTo(FavoriteDTOs().First()).Using(new FavoriteDTOEqualityComparer()));
        }
    }
}
