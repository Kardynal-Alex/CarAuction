
using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models
{
    [ExportTsClass]
    public class FavoriteViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int LotId { get; set; }
    }
}
