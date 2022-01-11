using Auction.BLL.DTO;
using Auction.DAL.Entities;
using AutoMapper;

namespace Auction.BLL.Mapping
{
    /// <summary>
    /// Automapper profile class in bll level
    /// </summary>
    public class AutomapperProfileBLL : Profile
    {
        /// <summary>
        /// AutomapperProfile ctor
        /// </summary>
        public AutomapperProfileBLL()
        {
            CreateMap<LotState, LotStateDTO>().ReverseMap();
            CreateMap<Lot, LotDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Images, ImagesDTO>().ReverseMap();
            CreateMap<AskOwner, AskOwnerDTO>().ReverseMap();
        }
    }
}
