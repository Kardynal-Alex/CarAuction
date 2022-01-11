using Auction.DAL.Entities;
using AutoMapper;
using PDFGenerator.Models;

namespace PDFGenerator.Mapping
{
    /// <summary>
    /// Automapper profile class in pdf level
    /// </summary>
    public class AutomapperProfilePDF : Profile
    {
        /// <summary>
        /// AutomapperProfile ctor
        /// </summary>
        public AutomapperProfilePDF()
        {
            CreateMap<Lot, LotData>().ReverseMap();
            CreateMap<Images, ImagesData>().ReverseMap();
            CreateMap<User, UserData>().ReverseMap();
        }
    }
}
