using Auction.BLL.Configure;
using Auction.DAL.Configure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auction.DAL.Entities;
using PDFGenerator.Configure;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Auction.WepApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                 {
                     builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                 });
            });

            ConfigureAspnetRunServices(services);
          
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 44344;
            });
           
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen();

            //Use global exeption middleware
            //services.AddTransient<ExceptionHandlingMiddleware>();
        }
        private void ConfigureAspnetRunServices(IServiceCollection services)
        {
            //DAL Dependencies
            services.AddDALDependencies(Configuration);
            var emailConfig = Configuration
                                .GetSection("EmailConfiguration")
                                    .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            //BLL Dependencies
            services.AddBLLDependencies(Configuration);
            //PDF Dependencies
            services.AddPDFDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Use global exeption middleware
            //app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors(options => options.AllowAnyOrigin()
                                            .AllowAnyMethod()
                                                .AllowAnyHeader());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
