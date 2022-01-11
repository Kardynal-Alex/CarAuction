
namespace Auction.BLL.DTO
{
    /// <summary>
    /// <see cref="AuthResponseDTO"/> data transfer class
    /// </summary>
    public class AuthResponseDTO
    {
        /// <summary>
        /// AuthResponse check if auth is successful
        /// </summary>
        public bool IsAuthSuccessful { get; set; }
        /// <summary>
        /// AuthResponse ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// AuthResponse jwt token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Check if needed 2 step verification
        /// </summary>
        public bool Is2StepVerificationRequired { get; set; }
        /// <summary>
        /// AuthResponse Provider="Email"
        /// </summary>
        public string Provider { get; set; }
    }
}
