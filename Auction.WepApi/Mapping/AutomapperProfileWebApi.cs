using Auction.BLL.DTO;
using Auction.WepApi.Models;
using AutoMapper;

namespace Auction.WepApi.Mapping
{
    /// <summary>
    /// Automapper profile class in web api level
    /// </summary>
    public class AutomapperProfileWebApi : Profile
    {
        /// <summary>
        /// AutomapperProfile ctor
        /// </summary>
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
        }
    }
}
