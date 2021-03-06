using Auction.BLL.DTO;
using Auction.BLL.DTO.Identity;
using Auction.BLL.DTO.Lot;
using Auction.DAL.Entities;
using AutoMapper;

namespace Auction.BLL.Mapping
{
    public class AutomapperProfileBLL : Profile
    {
        public AutomapperProfileBLL()
        {
            CreateMap<LotState, LotStateDTO>().ReverseMap();
            CreateMap<Lot, LotDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Images, ImagesDTO>().ReverseMap();
            CreateMap<AskOwner, AskOwnerDTO>().ReverseMap();
            CreateMap<AuthorDescription, AuthorDescriptionDTO>().ReverseMap();
        }
    }
}
