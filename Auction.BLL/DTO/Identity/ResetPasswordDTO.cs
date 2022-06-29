namespace Auction.BLL.DTO.Identity
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
