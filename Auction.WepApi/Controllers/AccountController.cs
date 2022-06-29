using Auction.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Auction.WepApi.Logs;
using Auction.BLL.DTO.Identity;
using Auction.WepApi.Models.Identity;

namespace Auction.WepApi.Controllers
{
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
      
        [HttpGet("GetUserById")]
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
       
        [HttpPost("Login")]
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
     
        [HttpPost("TwoStepVerification")]
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
     
        [HttpPost("Register")]
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
       
        [HttpGet("EmailConfirmation")]
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
     
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await service.LogoutAsync();
            return Ok();   
        }
      
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                await service.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, logger);
                return BadRequest();
            }
        }
       
        [HttpPost("ForgotPassword")]
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
      
        [HttpPost("ResetPassword")]
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
      
        [HttpPost("Facebook")]
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
     
        [HttpPost("Google")]
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
