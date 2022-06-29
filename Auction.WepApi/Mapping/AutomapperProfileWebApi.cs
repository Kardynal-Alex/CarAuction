using Auction.BLL.DTO;
using Auction.BLL.DTO.FilterModels;
using Auction.BLL.DTO.Identity;
using Auction.BLL.DTO.Lot;
using Auction.WepApi.Models;
using Auction.WepApi.Models.FilterModels;
using Auction.WepApi.Models.Identity;
using Auction.WepApi.Models.Lot;
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
            CreateMap<PageRequestViewModel, PageRequest>().ReverseMap();
            CreateMap<ComplexFilterViewModel, ComplexFilter>().ReverseMap();
        }
    }
}
