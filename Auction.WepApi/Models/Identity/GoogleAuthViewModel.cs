using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Identity
{
    [ExportTsClass]
    public class GoogleAuthViewModel
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
    }
}
