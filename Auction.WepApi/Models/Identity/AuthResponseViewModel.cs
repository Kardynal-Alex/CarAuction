using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class AuthResponseViewModel
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public bool Is2StepVerificationRequired { get; set; }
        public string Provider { get; set; }
    }
}
