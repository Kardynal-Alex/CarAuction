using Auction.BLL.Configure;
using Auction.DAL.Configure;
using Auction.DAL.Entities;
using Auction.WepApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDFGenerator.Configure;
using System;

namespace Auction.Tests.WepApiTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);

            ConfigureAspnetRunServices(services);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureAspnetRunServices(IServiceCollection services)
        {
            //DAL Dependencies
            services.AddDALDependencies(Configuration);
            var emailConfig = Configuration.GetSection("EmailConfiguration")
                                           .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            //BLL Dependencies
            services.AddBLLDependencies(Configuration);

            //PDF Dependencies
            services.AddPDFDependencies();
        }
    }
}
