
using Auction.BLL.DTO.Lot;

namespace Auction.BLL.DTO.FilterModels
{
    public class PageRequest
    {
        public CarBrandDTO[] CarBrand { get; set; }
        public ComplexFilter[] ComplexFilter { get; set; }
    }
}
