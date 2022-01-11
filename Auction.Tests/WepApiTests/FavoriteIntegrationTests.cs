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
    public class FavoriteIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/favorite/";
        private HttpClient _client;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FavoriteController_GetFavoritesByUserId(string id)
        {
            var expected = GetAllFavorites().Where(x => x.UserId == id).OrderBy(x => x.Id).ToList();
            var httpResponse = await _client.GetAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<FavoriteViewModel>>(stringResponse).OrderBy(x => x.Id).ToList();

            Assert.AreEqual(actual.Count, expected.Count);
            Assert.That(actual, Is.EqualTo(expected).Using(new FavoriteViewModelEqualityComparer()));
        }

        private static IEnumerable<FavoriteViewModel> GetAllFavorites()
        {
            return new List<FavoriteViewModel>()
            {
                new FavoriteViewModel { Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 1 },
                new FavoriteViewModel { Id = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c", LotId = 1 },
                new FavoriteViewModel { Id = "cccccccc-cccc-cccc-cccc-cccccccccccc", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 },
                new FavoriteViewModel { Id = "dddddddd-dddd-dddd-dddd-dddddddddddd", UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c", LotId = 2 }
            };
        }

        [Test]
        public async Task FavoriteController_GetFavoriteByLotIdAndUserId()
        {
            var favoriteViewModel = new FavoriteViewModel
            {
                LotId = 1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            var expected = GetAllFavorites().FirstOrDefault(x => x.UserId == favoriteViewModel.UserId && x.LotId == favoriteViewModel.LotId);
            var content = new StringContent(JsonConvert.SerializeObject(favoriteViewModel), Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync(requestUri + "favorite/", content);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<FavoriteViewModel>(stringResponse);
            Assert.That(actual, Is.EqualTo(expected).Using(new FavoriteViewModelEqualityComparer()));
        }

        [Test]
        public async Task FavoriteController_AddFavorite()
        {
            var favoriteViewModel = new FavoriteViewModel
            {
                Id = "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee",
                LotId = 3,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            var content = new StringContent(JsonConvert.SerializeObject(favoriteViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var favorite = await context.Favorites
                    .FirstOrDefaultAsync(x => x.LotId == favoriteViewModel.LotId && x.UserId == favoriteViewModel.UserId);
                Assert.AreEqual(favoriteViewModel.Id, favorite.Id);
                Assert.AreEqual(favoriteViewModel.LotId, favorite.LotId);
                Assert.AreEqual(favoriteViewModel.UserId, favorite.UserId);
                Assert.AreEqual(5, context.Favorites.Count());
            }
        }

        [Test]
        public async Task FavoriteController_AddFavorite_ThrowExceptionIfModelIsIncorrect()
        {
            //LotId is negative number
            var favoriteViewModel = new FavoriteViewModel
            {
                LotId = -3,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            await CheckExceptionWhileAddNewFavorite(favoriteViewModel);

            //UserId is empty
            favoriteViewModel.LotId = 3;
            favoriteViewModel.UserId = "";
            await CheckExceptionWhileAddNewFavorite(favoriteViewModel);
        }

        private async Task CheckExceptionWhileAddNewFavorite(FavoriteViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task FavoriteController_DeleteFavoriteByUserIdAndLotId()
        {
            var favoriteViewModel = new FavoriteViewModel
            {
                LotId = 1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            var content = new StringContent(JsonConvert.SerializeObject(favoriteViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "deletepost/", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.Favorites.Count());
            }
        }

        [Test]
        public async Task FavoriteController_DeleteFavoriteByUserIdAndLotId_ThrowExceptionIfModelIsIncorrect()
        {
            //LotId is negative number
            var favoriteViewModel = new FavoriteViewModel
            {
                LotId = -1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            await CheckExceptionWhileDeleteFavorite(favoriteViewModel);

            //UserId is empty
            favoriteViewModel.LotId = 1;
            favoriteViewModel.UserId = "";
            await CheckExceptionWhileDeleteFavorite(favoriteViewModel);
        }

        private async Task CheckExceptionWhileDeleteFavorite(FavoriteViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "deletepost/", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task FavoriteController_DeleteFavoriteById(string id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.Favorites.Count());
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
