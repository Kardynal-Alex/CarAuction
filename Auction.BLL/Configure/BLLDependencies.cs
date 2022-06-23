using Auction.BLL.Interfaces;
using Auction.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auction.BLL.Configure
{
    public static class BLLDependencies
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = AuthOptions.ISSUER,
                    ValidAudience = AuthOptions.AUDIENCE,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                };
            });
            services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));
            services.Configure<GoogleAuthSettings>(Configuration.GetSection(nameof(GoogleAuthSettings)));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILotService, LotService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IAuthorDescriptionService, AuthorDescriptionService>();
            return services;
        }
    }
}
