using Auction.BLL.Configure;
using Auction.BLL.DTO;
using Auction.BLL.Services;
using Auction.BLL.Validation;
using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Auction.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Tests.BLLTests
{
  
    [TestFixture]
    public class UserServiceTests
    {

        [TestCase("5ae019a1-c312-4589-ab62-8b8a1fcb882c")]
        public async Task UserService_GetUserById(string userId)
        {
            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            mockUserStore.Setup(x => x.FindByIdAsync(userId, CancellationToken.None))
                .ReturnsAsync(Users().ElementAt(0));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserManager).Returns(userManager);

            var userService = new UserService(mockUnitOfWork.Object, 
                                            UnitTestHelper.CreateMapperProfileBLL(), 
                                            new Mock<IOptions<FacebookAuthSettings>>().Object,
                                            new Mock<IOptions<GoogleAuthSettings>>().Object);
            var user = await userService.GetUserByIdAsync(userId);
            var mappedUser = new AutoMapperHelper<UserDTO, User>().MapToType(user);
            Assert.That(mappedUser, Is.EqualTo(Users().ElementAt(0)).Using(new UserEqualityComparer()));
        }

        private static List<User> Users()
        {
            return new List<User>
            {
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",
                    Role = "user",
                    Email = "irakardinal@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
                },
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    UserName = "admin@gmail.com",
                    Surname = "Kardynal",
                    Role = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                }
            };
        }

        [TestCase("admin@gmail.com")]
        public async Task UserService_GetUserByEmail(string email)
        {
            var mockUserManager = GetUserManagerMock<User>();
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(Users().ElementAt(1)));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserManager).Returns(mockUserManager.Object);

            var userService = new UserService(mockUnitOfWork.Object,
                                            UnitTestHelper.CreateMapperProfileBLL(),
                                            new Mock<IOptions<FacebookAuthSettings>>().Object,
                                            new Mock<IOptions<GoogleAuthSettings>>().Object);
            var userDTO = await userService.GetUserByEmailAsync(email);

            var mappedUser = new AutoMapperHelper<UserDTO, User>().MapToType(userDTO);
            Assert.That(mappedUser, Is.EqualTo(Users().ElementAt(1)).Using(new UserEqualityComparer()));
        }

        [Test]
        public void UserService_GetUsersWithRoleUser()
        {
            var userManagerMock = GetUserManagerMock<User>();
            userManagerMock.Setup(u => u.Users).Returns(Users().AsQueryable());

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserManager)
                .Returns(userManagerMock.Object);
            var userService = new UserService(mockUnitOfWork.Object,
                                            UnitTestHelper.CreateMapperProfileBLL(),
                                            new Mock<IOptions<FacebookAuthSettings>>().Object,
                                            new Mock<IOptions<GoogleAuthSettings>>().Object);

            var users = userService.GetUsersWithRoleUser();
            var mappedUsers = new AutoMapperHelper<UserDTO, User>().MapToTypeList(users);
            Assert.That(mappedUsers, Is.EqualTo(Users().Where(x => x.Role == "user"))
                .Using(new UserEqualityComparer()));
        }

        Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    null);
        }

        Mock<RoleManager<TIdentityRole>> GetRoleManagerMock<TIdentityRole>() where TIdentityRole : IdentityRole
        {
            return new Mock<RoleManager<TIdentityRole>>(
                    new Mock<IRoleStore<TIdentityRole>>().Object,
                    new IRoleValidator<TIdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    null);
        }
    }
}
