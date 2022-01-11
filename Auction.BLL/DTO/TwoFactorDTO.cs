
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="TwoFactorDTO"/> data transfer class
    /// </summary>
    public class TwoFactorDTO
    {
        /// <summary>
        /// TwoFactorDTO Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// TwoFactorDTO Provider="Email"
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// TwoFactorDTO Token(one time password)
        /// </summary>
        public string Token { get; set; }
    }
}
