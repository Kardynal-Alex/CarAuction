
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="ResetPasswordDTO"/> data transfer class
    /// </summary>
    public class ResetPasswordDTO
    {
        /// <summary>
        /// ResetPasswordDTO NameLot
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ResetPasswordDTO Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ResetPasswordDTO Token
        /// </summary>
        public string Token { get; set; }
    }
}
