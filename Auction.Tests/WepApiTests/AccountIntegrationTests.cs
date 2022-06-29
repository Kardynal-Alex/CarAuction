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
using Auction.WepApi.Models.Identity;

namespace Auction.Tests.WepApiTests
{
    [TestFixture]
    public class AccountIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/account/";
        private HttpClient _client;
        private static List<UserViewModel> AllUsers;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserViewModel>());
                var mapper = new Mapper(config);
                AllUsers = mapper.Map<List<UserViewModel>>(context.Users.ToList());
            }
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task AccountController_GetUserById(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getuserbyid/?id=" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserViewModel>(stringResponse);

            Assert.That(actual, Is.EqualTo(AllUsers.FirstOrDefault(x => x.Id == id))
                .Using(new UserViewModelEqualityComparer()));
        }
    }
}
