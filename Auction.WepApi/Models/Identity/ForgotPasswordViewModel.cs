using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
        public string ClientURI { get; set; }
    }
}
