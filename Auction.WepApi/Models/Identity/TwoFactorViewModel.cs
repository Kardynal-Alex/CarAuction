using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class TwoFactorViewModel
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
