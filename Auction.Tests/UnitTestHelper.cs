using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using System.Threading.Tasks;
using Moq;
using AutoMapper;
using Auction.BLL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Auction.WepApi.Mapping;
using PDFGenerator.Mapping;
using IdentityConstants = Auction.DAL.EF.IdentityConstants;

namespace Auction.Tests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<ApplicationContext> GetUnitDbOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new ApplicationContext(options))
            {
                SeedData(context);
            }

            return options;
        }
       
        public static void SeedData(ApplicationContext context)
        {
            context.Lots.Add(new Lot
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
            });
            context.LotStates.Add(new LotState
            {
                Id = 1,
                OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                LotId = 1,
                CountBid = 1
            });
            context.Lots.Add(new Lot
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
            });
            context.LotStates.Add(new LotState
            {
                Id = 2,
                OwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FutureOwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                LotId = 2,
                CountBid = 0
            });
            context.Lots.Add(new Lot
            {
                Id = 3,
                NameLot = "2016 Jaguar F-Type R Convertible",
                StartPrice = 41000,
                IsSold = false,
                Image = "Resources\\Images\\rGAe8wXw.NRKkRygVr-(edit).jpg",
                Description = "~33,700 Miles, AWD, Supercharged V8, Red Interior",
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                StartDateTime = DateTime.Parse("2021-06-15 07:36:36.9310000"),
                CurrentPrice = 41000,
                Year = 2016
            });
            context.LotStates.Add(new LotState
            {
                Id = 3,
                OwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                FutureOwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                LotId = 3,
                CountBid = 0
            });
            context.Lots.Add(new Lot
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
                Year = 2010
            });
            context.LotStates.Add(new LotState
            {
                Id = 4,
                OwnerId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                FutureOwnerId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                LotId = 4,
                CountBid = 3
            });
            context.Comments.Add(new Comment
            {
                Id = Guid.Parse("23f48b17-e073-f708-dcaa-5182ac94fd44"),
                Author = "Ira Kardynal",
                Text = "Bid $26000",
                DateTime = DateTime.Parse("2021-06-15 11:33:27.8151988"),
                LotId = 1,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = true
            });
            context.Comments.Add(new Comment
            {
                Id = Guid.Parse("714c8742-ba41-12e4-2964-d216efc641c6"),
                Author = "Oleksandr Kardynal",
                Text = "Cool car!",
                DateTime = DateTime.Parse("2021-06-15 11:08:42.1799046"),
                LotId = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                IsBid = false
            });
            context.Comments.Add(new Comment
            {
                Id = Guid.Parse("766874f8-2d32-1b3d-8f7a-1bd7c1351618"),
                Author = "Ira Kardynal",
                Text = "Bid $16000",
                DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                LotId = 2,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                IsBid = true
            });
            context.Comments.Add(new Comment
            {
                Id = Guid.Parse("ed4301c8-1ec2-7171-6b40-900fbf7c0a71"),
                Author = "Oleksandr Kardynal",
                Text = "Fantastic",
                DateTime = DateTime.Parse("2021-06-16 14:35:01.0129277"),
                LotId = 2,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                IsBid = false
            });
            context.Favorites.AddRange(
                new Favorite { Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 1 },
                new Favorite { Id = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c", LotId = 1 },
                new Favorite { Id = "cccccccc-cccc-cccc-cccc-cccccccccccc", UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", LotId = 2 },
                new Favorite { Id = "dddddddd-dddd-dddd-dddd-dddddddddddd", UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c", LotId = 2 });
            //BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);
            var users = new List<User>
            {
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    UserName = "admin@gmail.com",
                    Surname = "Kardynal",
                    Role = IdentityConstants.Admin,
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                },
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",
                    Role = IdentityConstants.User,
                    Email = "irakardinal@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
                }
            }.AsQueryable();
            context.Roles.Add(new IdentityRole { Id = "105695ec-0e70-4e43-8514-8a0710e11d53", Name = IdentityConstants.User });
            context.Roles.Add(new IdentityRole { Id = "012095ec-0e70-4e43-8514-8a0710e11d53", Name = IdentityConstants.Admin });
            context.UserRoles.Add(new IdentityUserRole<string> 
            { 
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53", 
                RoleId = "105695ec-0e70-4e43-8514-8a0710e11d53" 
            });
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                RoleId = "012095ec-0e70-4e43-8514-8a0710e11d53"
            });
            context.Users.AddRange(users);
            context.Images.Add(new Images
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
            });
            context.Images.Add(new Images
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
            });
            context.SaveChanges();
        }

        public static Mapper CreateMapperProfileBLL()
        {
            var myProfileBLL = new AutomapperProfileBLL();
            var myProfilePDF = new AutomapperProfilePDF();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(myProfileBLL);
                cfg.AddProfile(myProfilePDF);
            });

            return new Mapper(configuration);
        }

        public static Mapper CreateMapperProfileWebApi()
        {
            var myProfile = new AutomapperProfileWebApi();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}
