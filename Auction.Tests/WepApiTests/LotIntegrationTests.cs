using Auction.WepApi.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Auction.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Auction.DAL.Entities;
using AutoMapper;

namespace Auction.Tests.WepApiTests
{
    [TestFixture]
    public class LotIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/lot/";
        private HttpClient _client;
        private static List<LotViewModel> AllLots;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotViewModel>());
                var mapper = new Mapper(config);
                AllLots = mapper.Map<List<LotViewModel>>(context.Lots.ToList());
            }
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotController_GetLotsByUserId(string id)
        {
            var expected = AllLots.Where(x => x.UserId == id).OrderBy(x => x.Id).ToList();
            var httpResponse = await _client.GetAsync(requestUri + "getuserlots/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LotViewModel>>(stringResponse).OrderBy(x => x.Id).ToList();

            Assert.AreEqual(actual.Count, expected.Count);
            Assert.That(actual, Is.EqualTo(expected).Using(new LotViewModelEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotController_GetFavoriteLotsByUserId(string id)
        {
            var expected = GetFavoriteLotsByUserId();
            var httpResponse = await _client.GetAsync(requestUri + "favorites/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LotViewModel>>(stringResponse).OrderBy(x => x.Id).ToList();

            Assert.AreEqual(actual.Count, expected.Count);
            Assert.That(actual, Is.EqualTo(expected).Using(new LotViewModelEqualityComparer()));
        }

        private static List<LotViewModel> GetFavoriteLotsByUserId()
        {
            return new List<LotViewModel>
            {
                new LotViewModel
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
                new LotViewModel
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
        }

        [Test]
        public async Task LotController_GetSoldLots()
        {
            var expected = AllLots.Where(x => x.IsSold == true).OrderBy(x => x.Id).ToList();
            var httpResponse = await _client.GetAsync(requestUri + "getsoldlots/");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LotViewModel>>(stringResponse).OrderBy(x => x.Id).ToList();

            Assert.AreEqual(actual.Count, expected.Count);
            Assert.That(actual, Is.EqualTo(expected).Using(new LotViewModelEqualityComparer()));
        }

        [Test]
        public async Task LotController_GetAllLots()
        {
            var expected = AllLots.Where(x => x.IsSold == false).OrderBy(x => x.Id).ToList();
            var httpResponse = await _client.GetAsync(requestUri);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LotViewModel>>(stringResponse).OrderBy(x => x.Id).ToList();

            Assert.AreEqual(actual.Count, expected.Count);
            Assert.That(actual, Is.EqualTo(expected).Using(new LotViewModelEqualityComparer()));
        }

        #region oneregion1
        [TestCase(1)]
        public async Task LotController_GetLotByIdWithDetails(int id)
        {
            var expected = AllLots.FirstOrDefault(x => x.Id == id);
            var httpResponse = await _client.GetAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<LotViewModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(expected)
                .Using(new LotViewModelEqualityComparer()));
            Assert.That(actual.LotState, Is.EqualTo(GetLotStateForLotById())
                .Using(new LotStateViewModelEqualityComparer()));
            Assert.That(actual.User, Is.EqualTo(GetUserForLotById())
                .Using(new UserViewModelEqualityComparer()));
            Assert.That(actual.Images, Is.EqualTo(GetImagesForLotById())
                .Using(new ImagesViewModelEqualityComparer()));
        }

        private static UserViewModel GetUserForLotById()
        {
            return new UserViewModel
            {
                Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Name = "Oleksandr",
                Surname = "Kardynal",
                Role = "admin",
                Email = "admin@gmail.com"
            };
        }

        private static LotStateViewModel GetLotStateForLotById()
        {
            return new LotStateViewModel
            {
                Id = 1,
                OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                LotId = 1,
                CountBid = 1
            };
        }

        private static ImagesViewModel GetImagesForLotById()
        {
            return new ImagesViewModel
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
        }
        #endregion

        #region oneregion2
        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LotController_GetUserBids(string id)
        {
            var expected = ExpectedUserBids();
            var httpResponse = await _client.GetAsync(requestUri + "userbids/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LotViewModel>>(stringResponse).ToList();

            Assert.That(actual, Is.EqualTo(expected)
                .Using(new LotViewModelEqualityComparer()));
            Assert.That(actual.Select(x => x.User), Is.EqualTo(ExpectedUserBids().Select(x => x.User))
                .Using(new UserViewModelEqualityComparer()));
        }

        private static List<LotViewModel> ExpectedUserBids()
        {
            return new List<LotViewModel>
            {
                new LotViewModel
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
                    User = new UserViewModel
                    {
                        Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                        Name = "Ira",
                        Surname = "Kardynal",
                        Role = "user",
                        Email = "irakardinal@gmail.com"
                    }
                }
            };
        }
        #endregion

        [Test]
        public async Task LotController_AddLot()
        {
            var lotViewModel = new LotViewModel
            {
                Id = 5,
                NameLot = "2020 Nissan 370Z Roadster",
                StartPrice = 12200,
                IsSold = true,
                Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-edit.jpg",
                Description = "~53,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                StartDateTime = DateTime.Parse("2021-06-25 07:38:27.6820000"),
                CurrentPrice = 12200,
                Year = 2020,
                Images = new ImagesViewModel
                {
                    Id = 5,
                    Image1 = "Resources\\Images\\111-1.jpg",
                    Image2 = "Resources\\Images\\111-2.jpg",
                    Image3 = "Resources\\Images\\111-3.jpg",
                    Image4 = "Resources\\Images\\111-4.jpg",
                    Image5 = "Resources\\Images\\111-5.jpg",
                    Image6 = "Resources\\Images\\111-6.jpg",
                    Image7 = "Resources\\Images\\111-7.jpg",
                    Image8 = "Resources\\Images\\111-8.jpg",
                    Image9 = "Resources\\Images\\111-9.jpg"
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(lotViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var lot = await context.Lots.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == lotViewModel.Id);

                var config = new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<Lot, LotViewModel>();
                    cfg.CreateMap<Images, ImagesViewModel>();
                });
                var mapper = new Mapper(config);
                var mappedLot = mapper.Map<Lot, LotViewModel>(lot);

                Assert.That(mappedLot, Is.EqualTo(lotViewModel)
                    .Using(new LotViewModelEqualityComparer()));
                Assert.That(mappedLot.Images, Is.EqualTo(lotViewModel.Images)
                    .Using(new ImagesViewModelEqualityComparer()));
            }
        }

        [Test]
        public async Task LotController_AddLot_ThrowExceptionIfModelIsIncorrect()
        {
            //NameLot Is empty
            var lotViewModel = new LotViewModel
            {
                Id = 5,
                NameLot = "",
                StartPrice = 12200,
                IsSold = true,
                Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-edit.jpg",
                Description = "~53,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                StartDateTime = DateTime.Parse("2021-06-25 07:38:27.6820000"),
                CurrentPrice = 12200,
                Year = 2020
            };
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //StartPrice is negative number 
            lotViewModel.NameLot = "2020 Nissan 370Z Roadster";
            lotViewModel.StartPrice = -10;
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //Start Price is more than current price
            lotViewModel.StartPrice = 12200;
            lotViewModel.CurrentPrice = 11200;
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //Image path is empty
            lotViewModel.CurrentPrice = 12200;
            lotViewModel.Image = "";
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //UserId is empty
            lotViewModel.Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-edit.jpg";
            lotViewModel.UserId = "";
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //Current price is negative number
            lotViewModel.UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c";
            lotViewModel.CurrentPrice = -10;
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //Description is empty
            lotViewModel.CurrentPrice = 12200;
            lotViewModel.Description = "";
            await CheckExceptionWhileAddNewLot(lotViewModel);

            //Incorect year length
            lotViewModel.Description = "~53,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report";
            lotViewModel.Year = 123;
            await CheckExceptionWhileAddNewLot(lotViewModel);
        }

        private async Task CheckExceptionWhileAddNewLot(LotViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri, content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task LotController_UpdateOnlyDateLotToCurrentDate()
        {
            var lotViewModel = new LotViewModel
            {
                Id = 4,
                NameLot = "2020 Nissan 370Z Roadster",
                StartPrice = 12200,
                IsSold = false,
                Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-edit.jpg",
                Description = "~53,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                StartDateTime = DateTime.Parse("2021-06-25 07:38:27.6820000"),
                CurrentPrice = 13200,
                Year = 2020
            };

            var content = new StringContent(JsonConvert.SerializeObject(lotViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(requestUri + "onlydatelot", content);

            httpResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task LotController_UpdateLot()
        {
            var lotViewModel = new LotViewModel
            {
                Id = 4,
                NameLot = "2020 Nissan 370Z Roadster",
                StartPrice = 12200,
                IsSold = false,
                Image = "Resources\\Images\\3gNxG2JP.zricqvXXt-edit.jpg",
                Description = "~53,000 Miles, Touring Trim, 332-hp V6, Clean Carfax Report",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                StartDateTime = DateTime.Parse("2021-06-25 07:38:27.6820000"),
                CurrentPrice = 13200,
                Year = 2020
            };

            var content = new StringContent(JsonConvert.SerializeObject(lotViewModel), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(requestUri, content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var lot = await context.Lots.FindAsync(lotViewModel.Id);
                var mappedLot = new AutoMapperHelper<Lot, LotViewModel>().MapToType(lot);

                Assert.That(mappedLot, Is.EqualTo(lotViewModel).Using(new LotViewModelEqualityComparer()));
            }
        }

        [TestCase(1)]
        public async Task LotController_DeleteLotById(int id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.Lots.Count());
            }
        }
    }
}
