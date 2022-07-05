using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.Lot
{
    [ExportTsClass]
    public class AuthorDescriptionViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LotId { get; set; }
        public string Description { get; set; }
    }
}
