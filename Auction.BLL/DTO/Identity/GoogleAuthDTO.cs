namespace Auction.BLL.DTO.Identity
{
    public class GoogleAuthDTO
    {
        /// <summary>
        /// GoogleAuth Provider="google"
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// GoogleAuth IdToken
        /// </summary>
        public string IdToken { get; set; }
    }
}
