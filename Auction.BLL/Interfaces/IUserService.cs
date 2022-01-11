using Auction.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    /// <summary>
    /// IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JWT token</returns>
        Task<AuthResponseDTO> LoginAsync(UserDTO model);
        /// <summary>
        /// Verify one time password for login
        /// </summary>
        /// <param name="twoFactorDTO"></param>
        /// <returns></returns>
        Task<AuthResponseDTO> TwoStepVerificationAsync(TwoFactorDTO twoFactorDTO);
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Boolean result</returns>
        Task<bool> RegisterAsync(UserDTO model);
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserDTO> GetUserByEmailAsync(string email);
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDTO> GetUserByIdAsync(string id);
        /// <summary>
        /// DeleteUser by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(string userId);
        /// <summary>
        /// Get users with role user
        /// </summary>
        /// <returns></returns>
        List<UserDTO> GetUsersWithRoleUser();
        /// <summary>
        /// Regsiter with email confirmation
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task EmailConfirmationAsync(string email, string token);
        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="forgotPasswordDTO"></param>
        /// <returns></returns>
        Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        /// <summary>
        /// Facebook login
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<string> FacebookLoginAsync(string accessToken);
        /// <summary>
        /// Google Login
        /// </summary>
        /// <param name="googleAuthDTO"></param>
        /// <returns></returns>
        Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO);
    }
}
