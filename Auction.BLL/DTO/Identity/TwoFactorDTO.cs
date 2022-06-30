namespace Auction.BLL.DTO.Identity
{
    public class TwoFactorDTO
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
