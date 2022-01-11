using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using PDFGenerator.Shared;
using System.IO;

namespace PDFGenerator.Configure
{
    /// <summary>
    /// Add services connecting wid pdf generating
    /// </summary>
    public static class PDFDependencies
    {
        /// <summary>
        /// Extensions method add services in startup
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPDFDependencies(this IServiceCollection services)
        {
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            return services;
        }
    }
}
