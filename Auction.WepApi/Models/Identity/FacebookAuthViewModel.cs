using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class FacebookAuthViewModel
    {
        public string AccessToken { get; set; }
    }
}
