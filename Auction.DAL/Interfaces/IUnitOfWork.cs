using Auction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ILotRepository LotRepository { get; }
        ILotStateRepository LotStateRepository { get; }
        ICommentRepository CommentRepository { get; }
        IFavoriteRepository FavoriteRepository { get; }
        IEmailService EmailService { get; }
        IImagesRepository ImagesRepository { get; }
        IAuthorDescriptionRepository AuthorDescriptionRepository { get; }
        UserManager<User> UserManager { get; }
        SignInManager<User> SignInManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        Task SaveAsync();
    }
}
