using Auction.WepApi.Models.Lot;
using TypeGen.Core.TypeAnnotations;

namespace Auction.WepApi.Models.FilterModels
{
    [ExportTsClass]
    public class PageRequestViewModel
    {
        public CarBrandViewModel[] CarBrand { get; set; }
        public ComplexFilterViewModel[] ComplexFilter { get; set; }
    }
}
