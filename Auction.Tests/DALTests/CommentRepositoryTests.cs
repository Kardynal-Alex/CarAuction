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
    public class CommentRepositoryTests
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task CommentRepository_AddLot()
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);
            var comment = new Comment { Id = Guid.NewGuid() };

            await commentRepository.AddCommnetAsync(comment);
            await context.SaveChangesAsync();

            Assert.That(context.Comments.Count(), Is.EqualTo(5));
        }

        [TestCase("ed4301c8-1ec2-7171-6b40-900fbf7c0a71")]
        public async Task CommentRepository_DeleteCommentById(Guid commentId)
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);

            commentRepository.DeleteCommentById(commentId);
            await context.SaveChangesAsync();

            Assert.That(context.Comments.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task CommentRepository_DeleteCommentsInRange()
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);
            commentRepository.DeleteCommentsRange(ExpectedComments.ToList());
            await context.SaveChangesAsync();

            Assert.That(context.Comments.Count(), Is.EqualTo(2));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task CommentRepository_GetCommentsByLotId(int lotId)
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);
            var comments = await commentRepository.GetCommentsByLotIdAsync(lotId);
            var expected = await context.Comments.Where(x => x.LotId == lotId).ToListAsync();

            Assert.That(comments.Count(), Is.EqualTo(expected.Count()));
            Assert.That(comments.OrderBy(x => x.LotId), Is.EqualTo(expected.OrderBy(x => x.LotId))
                .Using(new CommentEqualityComparer()));
        }

        private static IEnumerable<Comment> ExpectedComments =>
            new[]
            {
                new Comment
                {
                    Id = Guid.Parse("766874f8-2d32-1b3d-8f7a-1bd7c1351618"),
                    Author = "Ira Kardynal",
                    Text = "Bid $16000",
                    DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                    LotId = 2,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    IsBid = true
                },
                new Comment
                {
                    Id = Guid.Parse("ed4301c8-1ec2-7171-6b40-900fbf7c0a71"),
                    Author = "Oleksandr Kardynal",
                    Text = "Fantastic",
                    DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                    LotId = 2,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    IsBid = false
                }
            };
    }
}
