using Auction.WepApi.Models.Lot;

namespace Auction.WepApi.Models.FilterModels
{
    public class PageRequestViewModel
    {
        public CarBrandViewModel[] CarBrand { get; set; }
        public ComplexFilterViewModel[] ComplexFilter { get; set; }
    }
}
