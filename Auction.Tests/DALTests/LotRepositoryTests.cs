using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using NUnit.Framework;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auction.Tests.DALTests
{
    [TestFixture]
    public class LotRepositoryTests
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task LotRepository_GetById(int id)
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var lot = await lotRepository.GetLotByIdAsync(id);

            var expected = ExpectedLots.FirstOrDefault(x => x.Id == id);
            Assert.That(lot, Is.EqualTo(expected).Using(new LotEqualityComparer()));
        }

        [Test]
        public async Task LotRepository_AddLot()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var lot = new Lot { Id = 10 };

            await lotRepository.AddLotAsync(lot);
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task LotRepository_DeleteLot()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            lotRepository.DeleteLotByLotId(4);
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task LotRepository_UpdateLot()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var lot = new Lot
            {
                Id = 1,
                NameLot = "2010 Mercedes-Benz SL63 AMG",
                StartPrice = 25550,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0f-(edit).jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-15 07:33:48.0630000"),
                CurrentPrice = 27000,
                Year = 2010
            };

            lotRepository.UpdateLot(lot);
            await context.SaveChangesAsync();

            Assert.That(lot, Is.EqualTo(new Lot
            {
                Id = 1,
                NameLot = "2010 Mercedes-Benz SL63 AMG",
                StartPrice = 25550,
                IsSold = false,
                Image = "Resources\\Images\\374JlnAD.5RHP3nx0f-(edit).jpg",
                Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                StartDateTime = DateTime.Parse("2021-06-15 07:33:48.0630000"),
                CurrentPrice = 27000,
                Year = 2010
            }).Using(new LotEqualityComparer()));
        }

        [Test]
        public async Task LotRepository_DeleteLotInRange()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            lotRepository.DeleteLotsRange(ExpectedLots.ToList());
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task LotRepository_GetAllLotsNotSold()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            var lots = await lotRepository.GetAllLotsAsync();
            var expectedLots = await context.Lots.Where(x => x.IsSold == false).ToListAsync();

            Assert.That(lots, Is.EqualTo(expectedLots).Using(new LotEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        [TestCase("5ae019a1-c312-4589-ab62-8b8a1fcb882c")]
        public async Task LotRepository_GetFavoriteLotsByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            var lots = await lotRepository.GetFavoriteLotsByUserIdAsync(userId);

            Assert.That(lots.OrderBy(x => x.Id), Is.EqualTo(ExpectedLots.ToList().OrderBy(x => x.Id))
                .Using(new LotEqualityComparer()));
        }

        [Test]
        public async Task LotRepository_GetSoldLots()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            var lots = await lotRepository.GetSoldLotsAsync();
            var expectedLots = await context.Lots.Where(x => x.IsSold == true).ToListAsync();

            Assert.That(lots, Is.EqualTo(expectedLots).Using(new LotEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotRepository_GetLotsByUserIdWithDetails(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            var lots = await lotRepository.GetLotsByUserIdAsync(userId);

            Assert.That(lots.OrderBy(x => x.Id), Is.EqualTo(ExpectedLots.OrderBy(x => x.Id))
                .Using(new LotEqualityComparer()));
            Assert.That(lots.Select(x => x.LotState).OrderBy(x => x.Id), Is.EqualTo(ExpectedLotStates)
                .Using(new LotStateEqualityComparer()));
            Assert.That(lots.Select(x => x.User).ElementAt(0), Is.EqualTo(ExpectedUsers.ElementAt(0))
                .Using(new UserEqualityComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task LotRepository_GetLotByIdWithDetails(int lotId)
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);

            var lot = await lotRepository.GetLotByIdWithDetailsAsync(lotId);

            Assert.That(lot, Is.EqualTo(ExpectedLots.ToList().ElementAt(lotId - 1))
                .Using(new LotEqualityComparer()));
            Assert.That(lot.LotState, Is.EqualTo(ExpectedLotStates.ToList().ElementAt(lotId - 1))
                .Using(new LotStateEqualityComparer()));
            Assert.That(lot.User, Is.EqualTo(ExpectedUsers.ElementAt(0))
                .Using(new UserEqualityComparer()));
            Assert.That(lot.Images, Is.EqualTo(ExpectedImages.ElementAt(lotId - 1))
                .Using(new ImagesEqualityComparer()));
        }

        private static IEnumerable<Lot> ExpectedLots =>
            new[]
            {
                new Lot
                {
                    Id = 1,
                    NameLot = "2009 Mercedes-Benz SL63 AMG",
                    StartPrice = 25550,
                    IsSold = false,
                    Image = "Resources\\Images\\374JlnAD.5RHP3nx0f-(edit).jpg",
                    Description = "518-hp V8, Southern-Kept, Accident-Free Carfax Report",
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    StartDateTime = DateTime.Parse("2021-06-15 07:33:48.0630000"),
                    CurrentPrice = 26000,
                    Year = 2009
                },
                new Lot
                {
                    Id = 2,
                    NameLot = "2020 Mercedes-AMG E63 S Wagon",
                    StartPrice = 45300,
                    IsSold = false,
                    Image = "Resources\\Images\\3OAWvVjv.6T8VjkQu4-(edit).jpg",
                    Description = "Factory Gulf Blue Color, 2 Owners, ~3,100 Miles, $45,300 in Options",
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    StartDateTime = DateTime.Parse("2021-06-15 07:34:34.2650000"),
                    CurrentPrice = 45300,
                    Year = 2020
                }
            };

        private static IEnumerable<User> ExpectedUsers =>
            new[]
            {
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    Surname = "Kardynal",
                    Role = IdentityConstants.Admin,
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com"
                }
            };

        private static IEnumerable<LotState> ExpectedLotStates =>
            new[]
            {
                new LotState
                {
                    Id = 1,
                    OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    LotId = 1,
                    CountBid = 1
                },
                new LotState
                {
                    Id = 2,
                    OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    FutureOwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    LotId = 2,
                    CountBid = 0
                }
            };

        private static IEnumerable<Images> ExpectedImages =>
            new[]
            {
                new Images
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
                },
                new Images
                {
                    Id = 2,
                    Image1 = "Resources\\Images\\22-1.jpg",
                    Image2 = "Resources\\Images\\22-2.jpg",
                    Image3 = "Resources\\Images\\22-3.jpg",
                    Image4 = "Resources\\Images\\22-4.jpg",
                    Image5 = "Resources\\Images\\22-5.jpg",
                    Image6 = "Resources\\Images\\22-6.jpg",
                    Image7 = "Resources\\Images\\22-7.jpg",
                    Image8 = "Resources\\Images\\22-8.jpg",
                    Image9 = "Resources\\Images\\22-9.jpg"
                }
            };

        [Test]
        public async Task LotRepository_AddAuthorDescription()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var authorDescription = new AuthorDescription { Id = 10 };

            await lotRepository.AddAuthorDescriptionAsync(authorDescription);
            await context.SaveChangesAsync();

            Assert.That(context.AuthorDescriptions.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task LotRepository_UpdateAuthorDescription()
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var authorDescription = new AuthorDescription 
            {
                Id = 1,
                LotId = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Description = "New Description 1"
            };

            lotRepository.UpdateAuthorDescription(authorDescription);
            await context.SaveChangesAsync();

            Assert.That(authorDescription, Is.EqualTo(new AuthorDescription
            {
                Id = 1,
                LotId = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Description = "New Description 1"
            }).Using(new AuthorDescriptionEqualityComparer()));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task LotRepository_GetAuthorDescriptionByLotId(int lotId)
        {
            await using var context = new ApplicationContext(_context);

            var lotRepository = new LotRepository(context);
            var lot = await lotRepository.GetAuthorDescriptionByLotIdAsync(lotId);

            var expected = ExpectedAuthorDescriptions.FirstOrDefault(x => x.LotId == lotId);
            Assert.That(lot, Is.EqualTo(expected).Using(new AuthorDescriptionEqualityComparer()));
        }

        private static IEnumerable<AuthorDescription> ExpectedAuthorDescriptions =>
           new[]
           {
                new AuthorDescription
                {
                    Id = 1,
                    LotId = 1,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Description = "Description 1"
                },
                new AuthorDescription
                {
                    Id = 2,
                    LotId = 2,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Description = "Description 2"
                }
           };

    }
}
