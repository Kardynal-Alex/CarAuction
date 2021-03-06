using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Lot
{
    [ExportTsClass]
    public class AskOwnerViewModel
    {
        public string OwnerEmail { get; set; }
        public string Text { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }
    }
}
