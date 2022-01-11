using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Tests.DALTests
{
    [TestFixture]
    public class FavoriteRepositoryTests
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task FavoriteRepository_AddFavorite()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);
            var favorite = new Favorite { Id = new Guid().ToString() };

            await favoriteRepository.AddFavoriteAsync(favorite);
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task FavoriteRepository_DeleteFavorite()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);

            favoriteRepository.DeleteFavorite("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task FavoriteRepository_DeleteFavoritesInRange()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);

            favoriteRepository.DeleteInRangeFavorites(ExpectedFavorites.ToList());
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task FavoriteRepository_DeleteFavoritesByLotIdAndUserId()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);

            await favoriteRepository.DeleteByLotIdAndUserIdAsync(new Favorite 
            { 
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 
            });
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task FavoriteRepository_GetFavoriteByUserIdAndLotId()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);
            var favorite = await favoriteRepository.GetFavoriteByUserIdAndLotIdAsync(new Favorite
            {
                Id = "cccccccc-cccc-cccc-cccc-cccccccccccc",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                LotId = 2
            });

            Assert.That(favorite.Id, Is.EqualTo("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        [TestCase("5ae019a1-c312-4589-ab62-8b8a1fcb882c")]
        public async Task FavoriteRepository_GetFavoritesByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);
            var favorites = await favoriteRepository.GetFavoriteByUserIdAsync(userId);
            var expected = await context.Favorites.Where(x => x.UserId == userId).ToListAsync();

            Assert.That(favorites.OrderBy(x => x.Id), Is.EqualTo(expected.OrderBy(x => x.Id))
                .Using(new FavoriteEqualityComparer()));
        }

        private static IEnumerable<Favorite> ExpectedFavorites =>
            new[]
            {
                new Favorite { Id = "cccccccc-cccc-cccc-cccc-cccccccccccc", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 },
                new Favorite { Id = "dddddddd-dddd-dddd-dddd-dddddddddddd", UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c", LotId = 2 }
            };
    }
}
