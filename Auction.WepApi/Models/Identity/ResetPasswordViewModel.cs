using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class ResetPasswordViewModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
