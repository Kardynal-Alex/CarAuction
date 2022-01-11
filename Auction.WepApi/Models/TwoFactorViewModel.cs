
namespace Auction.WepApi.Models
{
    /// <summary>
    /// TwoFactorViewModel
    /// </summary>
    public class TwoFactorViewModel
    {
        /// <summary>
        /// TwoFactorViewModel Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// TwoFactorViewModel provider="Email"
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// TwoFactorViewModel one time password token
        /// </summary>
        public string Token { get; set; }
    }
}
