using Auction.BLL.Configure;
using Auction.BLL.DTO;
using Auction.BLL.Interfaces;
using Auction.BLL.Validation;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityConstants = Auction.DAL.EF.IdentityConstants;

namespace Auction.BLL.Services
{
    /// <summary>
    /// User Service class
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly FacebookAuthSettings fbAuthSettings;
        private readonly GoogleAuthSettings googleAuthSettings;
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
            IOptions<GoogleAuthSettings> googlelAuthSettingsAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            fbAuthSettings = fbAuthSettingsAccessor.Value;
            googleAuthSettings = googlelAuthSettingsAccessor.Value;
        }
        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Boolean result</returns>
        public async Task<bool> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) 
                throw new AuctionException("Invalid user id");

            var user = await unitOfWork.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userLots = await unitOfWork.LotRepository.GetLotsByUserIdAsync(userId);
                var userLotStates = await unitOfWork.LotStateRepository.GetUserLotstatesAsync(userId);
                var userFavorites = await unitOfWork.FavoriteRepository.GetFavoriteByUserIdAsync(userId);
                unitOfWork.LotRepository.DeleteLotsRange(userLots);
                unitOfWork.LotStateRepository.DeleteLotStatesRange(userLotStates);
                unitOfWork.FavoriteRepository.DeleteInRangeFavorites(userFavorites);
                var result = await unitOfWork.UserManager.DeleteAsync(user);

                await unitOfWork.SaveAsync();
                return result.Succeeded;
            }
            throw new AuctionException("User is not founded!");
        }
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>UserDTO</returns>
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            if (email == null)
                throw new AuctionException("Invalid email");

            var user = await unitOfWork.UserManager.FindByEmailAsync(email);
            return mapper.Map<User, UserDTO>(user);
        }
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JWT Token</returns>
        public async Task<AuthResponseDTO> LoginAsync(UserDTO model)
        {
            if (model.Email == null || model.Password == null)
                throw new AuctionException("Incorect login or password");

            var user = await unitOfWork.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new AuctionException("User is not founded");

            var result = await unitOfWork.UserManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                await unitOfWork.UserManager.AccessFailedAsync(user);
                if(await unitOfWork.UserManager.IsLockedOutAsync(user))
                {
                    string errorMessage = "Your account is locked out!Please, reset password!";
                    var emailMessage = new EmailMessage
                    {
                        To = user.Email,
                        Content = string.Format("<h2 style='color:red;'>{0}</h2>", errorMessage),
                        Subject = "Locked out account information Car & Bids"
                    };
                    await unitOfWork.EmailService.SendEmailAsync(emailMessage);
                    return new AuthResponseDTO { ErrorMessage = "Password is locked out!" };
                }
                throw new AuctionException("Incorect login or password");
            }

            if (await unitOfWork.UserManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2StepVerification(user);

            await unitOfWork.UserManager.ResetAccessFailedCountAsync(user);

            var claims = await GetClaims(user.Email);
            var token = GenerateToken(claims);
            return new AuthResponseDTO { Token = token };
        }
        /// <summary>
        /// Generate one time password for 2 step verification
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<AuthResponseDTO> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await unitOfWork.UserManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                throw new AuctionException("Invalid 2-Step Verification Provider");
            }

            var token = await unitOfWork.UserManager.GenerateTwoFactorTokenAsync(user, "Email");
            var emailMessage = new EmailMessage
            {
                To = user.Email,
                Content = string.Format("<h2 style='color:red;'>{0}</h2>", token),
                Subject = "Authentication token Car & Bids"
            };
            await unitOfWork.EmailService.SendEmailAsync(emailMessage);

            return new AuthResponseDTO { Is2StepVerificationRequired = true, Provider = "Email" };
        }
        /// <summary>
        /// Verify one time password for login
        /// </summary>
        /// <param name="twoFactorDTO"></param>
        /// <returns></returns>
        public async Task<AuthResponseDTO> TwoStepVerificationAsync(TwoFactorDTO twoFactorDTO)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(twoFactorDTO.Email);
            if (user == null)
                throw new AuctionException("User is not founded");

            var validVerification = await unitOfWork.UserManager.VerifyTwoFactorTokenAsync(user, twoFactorDTO.Provider, twoFactorDTO.Token);
            if (!validVerification)
                throw new AuctionException("Invalid Token Verification");

            var claims = await GetClaims(user.Email);
            var token = GenerateToken(claims);
            return new AuthResponseDTO { Token = token };
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await unitOfWork.SignInManager.SignOutAsync();
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolean result</returns>
        public async Task<bool> RegisterAsync(UserDTO model)
        {
            if (model.Name == null || model.Surname == null || model.Role == null || model.Email == null || model.Password == null)
                throw new AuctionException("incorect register data");

            User user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Role = model.Role
            };
            var result = await unitOfWork.UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await unitOfWork.UserManager.AddToRoleAsync(user, model.Role);
                await unitOfWork.UserManager.SetTwoFactorEnabledAsync(user, true);
                var token = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
                var param = new Dictionary<string, string>
                {
                    {"token", token },
                    {"email", user.Email }
                };
                var callback = QueryHelpers.AddQueryString(model.ClientURI, param);
                await unitOfWork.EmailService.SendEmailAsync(new EmailMessage
                {
                    To = user.Email,
                    Subject = "Email confirmation token Car & Bids",
                    Content = string.Format("<h2 style='color:red;'>{0}</h2>", callback)
                });
                return true;
            }
            return false;
        }
        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserDTO</returns>
        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await unitOfWork.UserManager.FindByIdAsync(id);
            return mapper.Map<UserDTO>(user);
        }
        /// <summary>
        /// Get users with role user
        /// </summary>
        /// <returns></returns>
        public List<UserDTO> GetUsersWithRoleUser()
        {
            var users = unitOfWork.UserManager.Users.Where(x => x.Role.ToLower() == IdentityConstants.User).ToList();
            return mapper.Map<List<UserDTO>>(users);
        }
        /// <summary>
        /// Regsiter with email confirmation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task EmailConfirmationAsync(string email, string token)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null)
                throw new AuctionException("User is not founded");

            var confirmResult = await unitOfWork.UserManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                throw new AuctionException("Cannot confirm email");

        }
        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="forgotPasswordDTO"></param>
        /// <returns></returns>
        public async Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (forgotPasswordDTO.Email == null || forgotPasswordDTO.ClientURI == null) 
                throw new AuctionException("Icorrect input data");

            var user = await unitOfWork.UserManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null)
                throw new AuctionException("User is not founded");

            var token = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
                {
                    {"token", token },
                    {"email", user.Email }
                };
            var callback = QueryHelpers.AddQueryString(forgotPasswordDTO.ClientURI, param);
            await unitOfWork.EmailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email,
                Subject = "Reset Password token Car & Bids",
                Content = string.Format("<h2 style='color:red;'>{0}</h2>", callback)
            });
        }
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO.Email == null || resetPasswordDTO.Password == null || resetPasswordDTO.Token == null)
                throw new AuctionException("Invalid input data");

            var user = await unitOfWork.UserManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
                throw new AuctionException("User is not founded");

            var resetPassResult = await unitOfWork.UserManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);
            if (!resetPassResult.Succeeded)
                throw new AuctionException("Error");

            await unitOfWork.UserManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));
        }
        /// <summary>
        /// facebook login
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<string> FacebookLoginAsync(string accessToken)
        {
            HttpClient Client = new HttpClient();
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={fbAuthSettings.AppId}&client_secret={fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new AuctionException("Invalid facebook token");
            }
            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name&access_token={accessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = await unitOfWork.UserManager.FindByEmailAsync(userInfo.Email);
            if (user == null)
            {
                var newUser = new User
                {
                    Email = userInfo.Email,
                    Name = userInfo.FirstName,
                    Surname = userInfo.LastName,
                    UserName = userInfo.Email,
                    Role = IdentityConstants.User,
                    EmailConfirmed = true
                };
                var result = await unitOfWork.UserManager.CreateAsync(newUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));
                if (!result.Succeeded)
                    throw new AuctionException("Cannot create user");
                await unitOfWork.UserManager.AddToRoleAsync(newUser, newUser.Role);
            }

            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await unitOfWork.SignInManager.SignInAsync(user, properties); 

            var claims = await GetClaims(userInfo.Email);
            var token = GenerateToken(claims);
            return token;
        }
        /// <summary>
        /// GetUserClaims
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<List<Claim>> GetClaims(string email)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(email);

            if (user == null) 
                throw new AuctionException("user is not foundeed");
            else
            {
                var claims = new List<Claim>
                {
                    new Claim("id", user.Id),
                    new Claim("name", user.Name),
                    new Claim("surname", user.Surname),
                    new Claim("email", user.Email),
                    new Claim("role", user.Role)
                };
                return claims;
            }
        }
        /// <summary>
        /// GenerateToken
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateToken(List<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var tokenOptions = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
        /// <summary>
        /// Google login
        /// </summary>
        /// <param name="googleAuthDTO"></param>
        /// <returns></returns>
        public async Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO)
        {
            var payload = await VerifyGoogleToken(googleAuthDTO);
            if (payload == null)
                throw new AuctionException("Invalid Google Authentication");

            var info = new UserLoginInfo(googleAuthDTO.Provider, payload.Subject, googleAuthDTO.Provider);
            var user = await unitOfWork.UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await unitOfWork.UserManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        Role = IdentityConstants.User,
                        Name = payload.FamilyName,
                        Surname = payload.GivenName,
                        EmailConfirmed = payload.EmailVerified
                    };
                    var result = await unitOfWork.UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await unitOfWork.UserManager.AddToRoleAsync(user, user.Role);
                        await unitOfWork.UserManager.AddLoginAsync(user, info);
                    }
                    else
                    {
                        throw new AuctionException("Invalid External Authentication");
                    }
                }
                else
                {
                    await unitOfWork.UserManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                throw new AuctionException("Invalid External Authentication");

            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await unitOfWork.SignInManager.SignInAsync(user, properties);

            var claims = await GetClaims(user.Email);
            var token = GenerateToken(claims);
            return token;
        }
        /// <summary>
        /// Verify goole token
        /// </summary>
        /// <param name="externalAuth"></param>
        /// <returns></returns>
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthDTO externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { googleAuthSettings.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
