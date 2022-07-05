using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Lot
{
    [ExportTsClass]
    public class FutureOwner
    {
        public int LotId { get; set; }
        public string OwnerId { get; set; }
    }
}
