using Auction.DAL.EF;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Auction.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Auction.DAL.Configure
{
    /// <summary>
    /// Add services connecting wid dal level
    /// </summary>
    public static class DALDependencies
    {
        /// <summary>
        /// Extensions method add services in startup
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
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
