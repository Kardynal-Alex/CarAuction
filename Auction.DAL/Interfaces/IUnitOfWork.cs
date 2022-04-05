using Auction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.DAL.Interfaces
{
    /// <summary>
    /// Interface for accessing DB by repositories.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets lot repository.
        /// </summary>
        ILotRepository LotRepository { get; }
        /// <summary>
        /// Gets lot state repository.
        /// </summary>
        ILotStateRepository LotStateRepository { get; }
        /// <summary>
        /// Get comment repository.
        /// </summary>
        ICommentRepository CommentRepository { get; }
        /// <summary>
        /// Get favorite lot repository
        /// </summary>
        IFavoriteRepository FavoriteRepository { get; }
        /// <summary>
        /// Gets email service.
        /// </summary>
        IEmailService EmailService { get; }
        /// <summary>
        /// Gets images service
        /// </summary>
        IImagesRepository ImagesRepository { get; }
        /// <summary>
        /// Gets author description repositore
        /// </summary>
        IAuthorDescriptionRepository AuthorDescriptionRepository { get; }
        /// <summary>
        /// UserManager(identity).
        /// </summary>
        UserManager<User> UserManager { get; }
        /// <summary>
        /// SignInManager(identity).
        /// </summary>
        SignInManager<User> SignInManager { get; }
        /// <summary>
        /// RoleManager(identity).
        /// </summary>
        RoleManager<IdentityRole> RoleManager { get; }
        /// <summary>
        /// Async method for saving db changes.
        /// </summary>
        Task SaveAsync();
    }
}
