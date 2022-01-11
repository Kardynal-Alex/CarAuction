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
    public class ImagesRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task ImagesRepository_AddImages()
        {
            await using var context = new ApplicationContext(_context);

            var imagesRepository = new ImagesRepository(context);
            await imagesRepository.AddImagesAsync(new Images { Id = 10 });
            await context.SaveChangesAsync();

            Assert.AreEqual(3, context.Images.Count());
        }

        [TestCase(1)]
        public async Task ImagesRepository_DeleteImagesById(int id)
        {
            await using var context = new ApplicationContext(_context);

            var imagesRepository = new ImagesRepository(context);
            imagesRepository.DeleteImagesById(id);
            await context.SaveChangesAsync();

            Assert.AreEqual(1, context.Images.Count());
        }

        public async Task ImagesRepository_UpdateImages()
        {
            await using var context = new ApplicationContext(_context);

            var imagesRepository = new ImagesRepository(context);
            var images = new Images
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
            };
            imagesRepository.UpdateImages(images);
            await context.SaveChangesAsync();

            Assert.That(images, Is.EqualTo(new Images
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
            }).Using(new ImagesEqualityComparer()));
        }
    }
}
