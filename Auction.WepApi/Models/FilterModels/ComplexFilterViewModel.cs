using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.FilterModels
{
    [ExportTsClass]
    public class ComplexFilterViewModel
    {
        public string Field { get; set; }
        public SortOrderViewModel SortOrder { get; set; }
    }
}
