
namespace Auction.WepApi.Models
{
    /// <summary>
    /// ForgotPasswordViewModel
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// ForgotPasswordViewModel Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ForgotPasswordViewModel Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ForgotPasswordViewModel Token
        /// </summary>
        public string Token { get; set; }
    }
}
