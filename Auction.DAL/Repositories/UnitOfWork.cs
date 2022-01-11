using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    /// <summary>
    /// <see cref="IUnitOfWork"/> implementation class
    /// </summary
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        public UnitOfWork(ApplicationContext ctx, RoleManager<IdentityRole> _roleManager, 
                          UserManager<User> _userManager, SignInManager<User> _signInManager,
                          EmailConfiguration mailConfig)
        {
            context = ctx;
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
            EmailConfiguration = mailConfig;
        }
        /// <summary>
        /// Gets lot repository.
        /// </summary>
        private ILotRepository lotRepository;
        public ILotRepository LotRepository
        { 
            get
            {
                return lotRepository ?? (lotRepository = new LotRepository(context));
            }
        }
        /// <summary>
        /// Gets lotstate repository.
        /// </summary>
        private ILotStateRepository lotStateRepository;
        public ILotStateRepository LotStateRepository
        {
            get
            {
                return lotStateRepository ?? (lotStateRepository = new LotStateRepository(context));
            }
        }
        /// <summary>
        /// Gets comment repository
        /// </summary>
        private ICommentRepository commentRepository;
        public ICommentRepository CommentRepository
        {
            get
            {
                return commentRepository ?? (commentRepository = new CommentRepository(context));
            }
        }
        /// <summary>
        /// Get favorite repository
        /// </summary>
        private IFavoriteRepository favoriteRepository;
        public IFavoriteRepository FavoriteRepository
        {
            get
            {
                return favoriteRepository ?? (favoriteRepository = new FavoriteRepository(context));
            }
        }
        /// <summary>
        /// Get images repository
        /// </summary>
        private IImagesRepository imagesRepository;
        public IImagesRepository ImagesRepository
        {
            get
            {
                return imagesRepository ?? (imagesRepository = new ImagesRepository(context));
            }
        }
        /// <summary>
        /// Gets email service
        /// </summary>
        private readonly EmailConfiguration EmailConfiguration;
        private EmailService emailService;
        public IEmailService EmailService
        {
            get
            {
                return emailService ?? (emailService = new EmailService(EmailConfiguration));
            }
        }
        /// <summary>
        /// Gets userManager.
        /// </summary>
        private readonly UserManager<User> userManager;
        public UserManager<User> UserManager
        {
            get
            {
                return userManager;
            }
        }
        /// <summary>
        /// Gets singInManager.
        /// </summary>
        private readonly SignInManager<User> signInManager;
        public SignInManager<User> SignInManager
        {
            get
            {
                return signInManager;
            }
        }
        /// <summary>
        /// Gets roleManager.
        /// </summary>
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return roleManager;
            }
        }
        /// <summary>
        /// Save async
        /// </summary>
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
