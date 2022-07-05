using Auction.WepApi.Models.Identity;
using Auction.WepApi.Models.Lot;
using Auction.WepApi.Models.FilterModels;
using TypeGen.Core.SpecGeneration;

namespace Auction.WepApi.Models
{
    public class AuctionGenerationSpec : GenerationSpec
    {
        public AuctionGenerationSpec()
        {
            #region other
            AddClass<FavoriteViewModel>();
            AddClass<CommentViewModel>();
            #endregion
            #region lot
            AddClass<AskOwnerViewModel>(FolderName.LotPath);
            AddClass<AuthorDescriptionViewModel>(FolderName.LotPath);
            AddClass<FutureOwner>(FolderName.LotPath);
            AddClass<ImagesViewModel>(FolderName.LotPath);
            AddClass<LotStateViewModel>(FolderName.LotPath);
            AddEnum<CarBrandViewModel>(FolderName.LotPath);
            AddClass<LotViewModel>(FolderName.LotPath);
            #endregion
            #region filter
            AddEnum<SortOrderViewModel>(FolderName.FilterPath);
            AddClass<PageRequestViewModel>(FolderName.FilterPath);
            AddClass<ComplexFilterViewModel>(FolderName.FilterPath);
            #endregion
            #region auth
            AddClass<UserViewModel>(FolderName.AuthPath);
            AddClass<TwoFactorViewModel>(FolderName.AuthPath);
            AddClass<ResetPasswordViewModel>(FolderName.AuthPath);
            AddClass<GoogleAuthViewModel>(FolderName.AuthPath);
            AddClass<ForgotPasswordViewModel>(FolderName.AuthPath);
            AddClass<FacebookAuthViewModel>(FolderName.AuthPath);
            AddClass<AuthResponseViewModel>(FolderName.AuthPath);
            #endregion
        }
    }
    //Script to generate TypeScript models
    //TypeGen generate
    public static class FolderName
    {
        public static readonly string LotPath = "lot-models";
        public static readonly string AuthPath = "auth-models";
        public static readonly string FilterPath = "filter";
    } 
}
