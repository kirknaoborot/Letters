using Letters.Data;
using Letters.Service.Interfaces;
using Letters.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Letters.Service
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddDbContext<ReceptionContext>(options => options.UseNpgsql(configuration.GetConnectionString("ReceptionConnection")));

            services.AddTransient<ICaptchaService, CaptchaService>();
            services.AddTransient<IFormService, FormService>();

            services.AddMemoryCache();
        }
    }
}
