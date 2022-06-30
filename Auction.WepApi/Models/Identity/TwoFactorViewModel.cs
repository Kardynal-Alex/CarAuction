namespace Auction.WepApi.Models.Identity
{
    public class TwoFactorViewModel
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
