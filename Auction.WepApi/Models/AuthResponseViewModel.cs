
namespace Auction.WepApi.Models
{
    /// <summary>
    /// AuthResponseViewModel
    /// </summary>
    public class AuthResponseViewModel
    {
        /// <summary>
        /// AuthResponseViewModel If auth is successful
        /// </summary>
        public bool IsAuthSuccessful { get; set; }
        /// <summary>
        /// AuthResponseViewModel ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// AuthResponseViewModel Jwt token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// AuthResponseViewModel is 2 step verification required
        /// </summary>
        public bool Is2StepVerificationRequired { get; set; }
        /// <summary>
        /// AuthResponseViewModel provider="Email"
        /// </summary>
        public string Provider { get; set; }
    }
}
