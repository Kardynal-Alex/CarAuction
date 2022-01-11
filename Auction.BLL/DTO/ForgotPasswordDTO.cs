
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="ForgotPasswordDTO"/> data transfer class
    /// </summary>
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// ForgotPasswordDTO Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ForgotPasswordDTO ClientUri (PPL URI)
        /// </summary>
        public string ClientURI { get; set; }
    }
}
