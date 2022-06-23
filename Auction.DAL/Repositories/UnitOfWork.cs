using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        public UnitOfWork(
            ApplicationContext ctx, 
            RoleManager<IdentityRole> _roleManager, 
            UserManager<User> _userManager, 
            SignInManager<User> _signInManager,
            EmailConfiguration mailConfig
        )
        {
            context = ctx;
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
            EmailConfiguration = mailConfig;
        }
    
        private ILotRepository lotRepository;
        public ILotRepository LotRepository
        { 
            get
            {
                return lotRepository ?? (lotRepository = new LotRepository(context));
            }
        }
      
        private ILotStateRepository lotStateRepository;
        public ILotStateRepository LotStateRepository
        {
            get
            {
                return lotStateRepository ?? (lotStateRepository = new LotStateRepository(context));
            }
        }
       
        private ICommentRepository commentRepository;
        public ICommentRepository CommentRepository
        {
            get
            {
                return commentRepository ?? (commentRepository = new CommentRepository(context));
            }
        }
     
        private IFavoriteRepository favoriteRepository;
        public IFavoriteRepository FavoriteRepository
        {
            get
            {
                return favoriteRepository ?? (favoriteRepository = new FavoriteRepository(context));
            }
        }
       
        private IImagesRepository imagesRepository;
        public IImagesRepository ImagesRepository
        {
            get
            {
                return imagesRepository ?? (imagesRepository = new ImagesRepository(context));
            }
        }
     
        private readonly EmailConfiguration EmailConfiguration;
        private EmailService emailService;
        public IEmailService EmailService
        {
            get
            {
                return emailService ?? (emailService = new EmailService(EmailConfiguration));
            }
        }
     
        private AuthorDescriptionRepository authorDescriptionRepository;
        public IAuthorDescriptionRepository AuthorDescriptionRepository
        {
            get
            {
                return authorDescriptionRepository ?? (authorDescriptionRepository = new Repositories.AuthorDescriptionRepository(context));
            }
        }
        
        private readonly UserManager<User> userManager;
        public UserManager<User> UserManager
        {
            get
            {
                return userManager;
            }
        }
        
        private readonly SignInManager<User> signInManager;
        public SignInManager<User> SignInManager
        {
            get
            {
                return signInManager;
            }
        }
        
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return roleManager;
            }
        }
       
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
