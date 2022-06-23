using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Auction.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Auction.DAL.Configure
{
    public static class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(conString));

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILotRepository, LotRepository>();
            services.AddTransient<ILotStateRepository, LotStateRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IFavoriteRepository, FavoriteRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthorDescriptionRepository, AuthorDescriptionRepository>();

            return services;
        }
    }
}
