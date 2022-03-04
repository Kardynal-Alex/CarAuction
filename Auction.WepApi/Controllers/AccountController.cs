﻿using Auction.BLL.Interfaces;
using Auction.WepApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Auction.BLL.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Auction.WepApi.Logs;

namespace Auction.WepApi.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IMapper mapper;
        private readonly ILogger<AccountController> logger;
        public AccountController(IUserService srvc, IMapper mapper, ILogger<AccountController> logger)
        {
            service = srvc;
            this.mapper = mapper;
            this.logger = logger;
        }
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("getuser/{email}")]
        public async Task<ActionResult<UserViewModel>> GetUserByEmail(string email)
        {
            try
            {
                var user = await service.GetUserByEmailAsync(email);
                return Ok(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getuserbyid")]
        public async Task<ActionResult<UserViewModel>> GetUserById(string id)
        {
            try
            {
                var user = await service.GetUserByIdAsync(id);
                return Ok(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// login user
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseViewModel>> Login([FromBody]UserViewModel userViewModel)
        {
            try
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                var authResponseDTO = await service.LoginAsync(userDTO);
                var authResponseViewModel = mapper.Map<AuthResponseDTO, AuthResponseViewModel>(authResponseDTO);
                return Ok(authResponseViewModel);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Two step verification OTP confirmation
        /// </summary>
        /// <param name="twoFactorViewModel"></param>
        /// <returns></returns>
        [HttpPost("twoStepVerification")]
        public async Task<ActionResult<AuthResponseViewModel>> TwoStepVerification([FromBody] TwoFactorViewModel twoFactorViewModel)
        {
            try
            {
                var twoFactorDTO = mapper.Map<TwoFactorViewModel, TwoFactorDTO>(twoFactorViewModel);
                var authResponseDTO = await service.TwoStepVerificationAsync(twoFactorDTO);
                var authResponseViewModel = mapper.Map<AuthResponseDTO, AuthResponseViewModel>(authResponseDTO);
                if (authResponseViewModel.Token != null)
                {
                    return Ok(authResponseViewModel);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> Register([FromBody]UserViewModel userViewModel)
        {
            try
            {
                var userDTO = mapper.Map<UserViewModel, UserDTO>(userViewModel);
                var result = await service.RegisterAsync(userDTO);
                return Ok(userViewModel);
            }
            catch (Exception ex) 
            { 
                LogInfo.LogInfoMethod(ex, logger); 
                return BadRequest(); 
            }
        }
        /// <summary>
        /// For confirmation email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("emailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            try
            {
                await service.EmailConfirmationAsync(email, token);
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await service.LogoutAsync();
            return Ok();   
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                return Ok(await service.DeleteUser(id));
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="forgotPasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel forgotPasswordViewModel)
        {
            try
            {
                await service.ForgotPasswordAsync(mapper.Map<ForgotPasswordViewModel, ForgotPasswordDTO>(forgotPasswordViewModel));
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="resetPasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {
                await service.ResetPasswordAsync(mapper.Map<ResetPasswordViewModel, ResetPasswordDTO>(resetPasswordViewModel));
                return Ok();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// facebook login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("facebook")]
        public async Task<ActionResult> Facebook([FromBody] FacebookAuthViewModel model)
        {
            try
            {
                var tokenString = await service.FacebookLoginAsync(model.AccessToken);
                if (tokenString != null)
                {
                    return Ok(new { Token = tokenString });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
        /// <summary>
        /// Google login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("google")]
        public async Task<ActionResult> Google([FromBody] GoogleAuthViewModel model)
        {
            try
            {
                var googleAuthDTO = mapper.Map<GoogleAuthViewModel, GoogleAuthDTO>(model);
                var tokenString = await service.GoogleLoginAsync(googleAuthDTO);
                if (tokenString != null)
                {
                    return Ok(new { Token = tokenString });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
    }
}