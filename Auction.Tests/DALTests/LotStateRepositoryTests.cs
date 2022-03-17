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
    public class LotStateRepositoryTests
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task LotStateRepository_AddLotState()
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            var lotState = new LotState() { Id = 10 };

            lotStateRepository.AddLotState(lotState);
            await context.SaveChangesAsync();

            Assert.That(context.LotStates.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task LotStateRepository_DeleteLotState()
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);

            await lotStateRepository.DeleteLotStateByLotIdAsync(4);
            await context.SaveChangesAsync();

            Assert.That(context.LotStates.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task LotStateRepository_UpdateLotState()
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            var lotState = new LotState
            {
                Id = 2,
                OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                LotId = 2,
                CountBid = 4
            };

            lotStateRepository.UpdateLotState(lotState);
            await context.SaveChangesAsync();

            Assert.That(lotState, Is.EqualTo(new LotState
            {
                Id = 2,
                OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                LotId = 2,
                CountBid = 4
            }).Using(new LotStateEqualityComparer()));
        }

        [Test]
        public async Task LotStateRepository_DeleteLotStateInRange()
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            lotStateRepository.DeleteLotStatesRange(ExpectedLotStates.ToList());
            await context.SaveChangesAsync();

            Assert.That(context.LotStates.Count(), Is.EqualTo(2));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task LotStateRepository_FindLotStateByLotId(int lotId)
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            var lotState = await lotStateRepository.FindLotStateByLotIdAsync(lotId);

            var expected = ExpectedLotStates.ToList().ElementAt(lotId - 1);
            Assert.That(lotState, Is.EqualTo(expected).Using(new LotStateEqualityComparer()));
        }

        [Test]
        public async Task LotStateRepository_GetAllLotStates()
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            var expected = await lotStateRepository.GetAllLotStateAsync();

            Assert.That(context.LotStates.Count(), Is.EqualTo(expected.Count()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        [TestCase("5ae019a1-c312-4589-ab62-8b8a1fcb882c")]
        public async Task LotStateRepository_GetUserLotStates(string ownerId)
        {
            await using var context = new ApplicationContext(_context);

            var lotStateRepository = new LotStateRepository(context);
            var lotStates = await lotStateRepository.GetUserLotstatesAsync(ownerId);
            var expected = await context.LotStates.Where(x => x.OwnerId == ownerId).ToListAsync();

            Assert.That(lotStates.Count(), Is.EqualTo(expected.Count()));
            Assert.That(lotStates.OrderBy(x => x.Id), Is.EqualTo(expected.OrderBy(x => x.Id))
                .Using(new LotStateEqualityComparer()));
        }

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
   }
}
