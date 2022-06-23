using Auction.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.BLL.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDTO> LoginAsync(UserDTO model);
        Task<AuthResponseDTO> TwoStepVerificationAsync(TwoFactorDTO twoFactorDTO);
        Task<bool> RegisterAsync(UserDTO model);
        Task LogoutAsync();
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<bool> DeleteUser(string userId);
        List<UserDTO> GetUsersWithRoleUser();
        Task EmailConfirmationAsync(string email, string token);
        Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
        Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<string> FacebookLoginAsync(string accessToken);
        Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO);
    }
}
