using Auction.WepApi.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Auction.BLL.DTO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Auction.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Auction.Tests.WepApiTests
{
    [TestFixture]
    public class CommentIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/comment/";
        private HttpClient _client;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task CommentController_AddComment()
        {
            var commentViewModel = new CommentViewModel
            {
                Id = Guid.Parse("25f48b17-e073-f708-dcaa-5182ac94fd44"),
                Author = "Ira Kardynal",
                Text = "Bid $27000",
                DateTime = DateTime.Parse("2021-06-25 11:33:27.8151988"),
                LotId = "1",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(commentViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var comment = await context.Comments
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse("25f48b17-e073-f708-dcaa-5182ac94fd44"));
                Assert.AreEqual(commentViewModel.Id, comment.Id);
                Assert.AreEqual(commentViewModel.UserId, comment.UserId);
                Assert.AreEqual(commentViewModel.LotId, comment.LotId.ToString());
                Assert.AreEqual(commentViewModel.IsBid, comment.IsBid);
            }
        }

        public List<CommentViewModel> GetAllComments()
        {
            return new List<CommentViewModel>()
            {
                new CommentViewModel
                {
                    Id = Guid.Parse("23f48b17-e073-f708-dcaa-5182ac94fd44"),
                    Author = "Ira Kardynal",
                    Text = "Bid $26000",
                    DateTime = DateTime.Parse("2021-06-15 11:33:27.8151988"),
                    LotId = "1",
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    IsBid = true
                },
                new CommentViewModel
                {
                    Id = Guid.Parse("714c8742-ba41-12e4-2964-d216efc641c6"),
                    Author = "Oleksandr Kardynal",
                    Text = "Cool car!",
                    DateTime = DateTime.Parse("2021-06-15 11:08:42.1799046"),
                    LotId = "1",
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    IsBid = false
                },
                new CommentViewModel
                {
                    Id = Guid.Parse("766874f8-2d32-1b3d-8f7a-1bd7c1351618"),
                    Author = "Ira Kardynal",
                    Text = "Bid $16000",
                    DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                    LotId = "2",
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    IsBid = true
                },
                new CommentViewModel
                {
                     Id = Guid.Parse("ed4301c8-1ec2-7171-6b40-900fbf7c0a71"),
                    Author = "Oleksandr Kardynal",
                    Text = "Fantastic",
                    DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                    LotId = "2",
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    IsBid = false
                }
            };
        }

        [Test]
        public async Task CommentController_AddComment_ThrowExceptionIfModelIsIncorrect()
        {
            //Authror is empty
            var commentViewModel = new CommentViewModel
            {
                Id = Guid.Parse("25f48b17-e073-f708-dcaa-5182ac94fd44"),
                Author = "",
                Text = "Bid $27000",
                DateTime = DateTime.Parse("2021-06-25 11:33:27.8151988"),
                LotId = "1",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = true
            };
            await CheckExceptionWhileAddNewComment(commentViewModel);

            //Text is empty;
            commentViewModel.Author = "Ira Kardynal";
            commentViewModel.Text = "";
            await CheckExceptionWhileAddNewComment(commentViewModel);

            //LotId is not number
            commentViewModel.Text = "Bid $27000";
            commentViewModel.LotId = "1a";
            await CheckExceptionWhileAddNewComment(commentViewModel);

            //UserId is empty
            commentViewModel.LotId = "1";
            commentViewModel.UserId = "";
            await CheckExceptionWhileAddNewComment(commentViewModel);
        }

        private async Task CheckExceptionWhileAddNewComment(CommentViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("2")]
        public async Task CommentController_GetCommentByLotId(string id)
        {
            var expected = GetAllComments().Where(x => x.LotId == id).ToList();
            var httpResponse = await _client.GetAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CommentViewModel>>(stringResponse).ToList();

            Assert.That(actual, Is.EqualTo(expected).Using(new CommentViewModelEqualityComparer()));
        }

        [TestCase("23f48b17-e073-f708-dcaa-5182ac94fd44")]
        public async Task CommentController_DeleteCommentById(Guid id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.Comments.Count());
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
