using Auction.BLL.DTO;
using Auction.WepApi.Models;
using AutoMapper;

namespace Auction.WepApi.Mapping
{
    public class AutomapperProfileWebApi : Profile
    {
        public AutomapperProfileWebApi()
        {
            CreateMap<LotStateDTO, LotStateViewModel>().ReverseMap();
            CreateMap<UserDTO, UserViewModel>().ReverseMap();
            CreateMap<LotDTO, LotViewModel>().ReverseMap();
            CreateMap<FutureOwnerDTO, FutureOwner>().ReverseMap();
            CreateMap<CommentDTO, CommentViewModel>().ReverseMap();
            CreateMap<ForgotPasswordDTO, ForgotPasswordViewModel>().ReverseMap();
            CreateMap<ResetPasswordDTO, ResetPasswordViewModel>().ReverseMap();
            CreateMap<FavoriteDTO, FavoriteViewModel>().ReverseMap();
            CreateMap<ImagesDTO, ImagesViewModel>().ReverseMap();
            CreateMap<GoogleAuthDTO, GoogleAuthViewModel>().ReverseMap();
            CreateMap<AskOwnerDTO, AskOwnerViewModel>().ReverseMap();
            CreateMap<AuthResponseDTO, AuthResponseViewModel>().ReverseMap();
            CreateMap<TwoFactorDTO, TwoFactorViewModel>().ReverseMap();
            CreateMap<AuthorDescriptionDTO, AuthorDescriptionViewModel>().ReverseMap();
        }
    }
}
