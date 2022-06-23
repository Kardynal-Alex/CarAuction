using Auction.DAL.Entities;
using AutoMapper;
using PDFGenerator.Models;

namespace PDFGenerator.Mapping
{
    public class AutomapperProfilePDF : Profile
    {
        public AutomapperProfilePDF()
        {
            CreateMap<Lot, LotData>().ReverseMap();
            CreateMap<Images, ImagesData>().ReverseMap();
            CreateMap<User, UserData>().ReverseMap();
        }
    }
}
