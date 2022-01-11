using Auction.BLL.DTO;
using Auction.BLL.Services;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Tests.BLLTests
{
    [TestFixture]
    public class CommentServiceTests
    {
        [Test]
        public async Task CommentService_AddComment()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.AddCommnetAsync(It.IsAny<Comment>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());
            var commentDTO = new CommentDTO
            {
                Id = Guid.NewGuid(),
                Author = "Ira Kardynal",
                Text = "Wonderfull car ever",
                DateTime = DateTime.Parse("2021-06-18 14:35:01.0129277"),
                LotId = 1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = false
            };

            await commentSevice.AddCommentAsync(commentDTO);

            mockUnitOfWork.Verify(x => x.CommentRepository.AddCommnetAsync(It.Is<Comment>(
                x => x.Id == commentDTO.Id && x.Author == commentDTO.Author && x.Text == commentDTO.Text && x.DateTime == commentDTO.DateTime
                && x.LotId == commentDTO.LotId && x.UserId == commentDTO.UserId && x.IsBid == commentDTO.IsBid)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void CommentService_AddComment_ThrowsAuctionExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.AddCommnetAsync(It.IsAny<Comment>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var commentDTO = new CommentDTO
            {
                Id = Guid.NewGuid(),
                Author = "",
                Text = "Wonderfull car ever",
                DateTime = DateTime.Parse("2021-06-18 14:35:01.0129277"),
                LotId = 1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = false
            };
            //Author is empty
            Assert.ThrowsAsync<AuctionException>(async () => await commentSevice.AddCommentAsync(commentDTO));

            //Text comment is empty
            commentDTO.Author = "Ira Kardynal";
            commentDTO.Text = "";
            Assert.ThrowsAsync<AuctionException>(async () => await commentSevice.AddCommentAsync(commentDTO));

            //UserId is empty
            commentDTO.Text = "Wonderfull car ever";
            commentDTO.UserId = "";
            Assert.ThrowsAsync<AuctionException>(async () => await commentSevice.AddCommentAsync(commentDTO));

            //Lot id is negative number
            commentDTO.UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c";
            commentDTO.LotId = -10;
            Assert.ThrowsAsync<AuctionException>(async () => await commentSevice.AddCommentAsync(commentDTO));
        }

        [TestCase("ed4301c8-1ec2-7171-6b40-900fbf7c0a71")]
        [TestCase("714c8742-ba41-12e4-2964-d216efc641c6")]
        public async Task CommentService_DeleteComment(Guid commentId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.DeleteCommentById(It.IsAny<Guid>()));
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            await commentService.DeleteCommentByIdAsync(commentId);

            mockUnitOfWork.Verify(x => x.CommentRepository.DeleteCommentById(commentId), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        public async Task CommentService_GetCommentsByLotId(int lotId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.GetCommentsByLotIdAsync(It.IsAny<int>()))
                .Returns(ExpectedCommentsByLotId());
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            var commentDTOs = await commentService.GetCommentsByLotIdAsync(lotId);
            var expectedCommentDTOs = await ExpectedCommentDTOsByLotId();

            Assert.IsInstanceOf<List<CommentDTO>>(commentDTOs);
            Assert.That(commentDTOs, Is.EqualTo(expectedCommentDTOs).Using(new CommentDTOEqualityComparer()));
        }

        [TestCase(-1)]
        public void CommentService_GetCommentsByLotId_ThrowAuctionExceptionIfLotIdIsNegNum(int lotId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.GetCommentsByLotIdAsync(It.IsAny<int>()))
                .Returns(ExpectedCommentsByLotId());
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfileBLL());

            Assert.ThrowsAsync<AuctionException>(async () => await commentService.GetCommentsByLotIdAsync(lotId));
        }

        private static Task<List<Comment>> ExpectedCommentsByLotId()
        {
            return Task.FromResult(new List<Comment>
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
            });
        }
        private static Task<List<CommentDTO>> ExpectedCommentDTOsByLotId()
        {
            return Task.FromResult(new List<CommentDTO>
            {
                new CommentDTO
                {
                    Id = Guid.Parse("23f48b17-e073-f708-dcaa-5182ac94fd44"),
                    Author = "Ira Kardynal",
                    Text = "Bid $26000",
                    DateTime = DateTime.Parse("2021-06-15 11:33:27.8151988"),
                    LotId = 1,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    IsBid = true
                },
                new CommentDTO
                {
                    Id = Guid.Parse("714c8742-ba41-12e4-2964-d216efc641c6"),
                    Author = "Oleksandr Kardynal",
                    Text = "Cool car!",
                    DateTime = DateTime.Parse("2021-06-15 11:08:42.1799046"),
                    LotId = 1,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    IsBid = false
                }
            });
        }
    }
}
