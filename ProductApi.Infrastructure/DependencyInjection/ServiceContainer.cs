using ECommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService
            (this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database connectivity
            // Add Authentication Scheme

            SharedServiceContainer.AddSharedService<ProductDbContext>
                (services, configuration, configuration["MySerilog:FileName"]!);

            // Create DI
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy
            (this IApplicationBuilder app)
        {
            // Register middleware such as:
            // Global Exception Handler: handles external errors
            // Listen to Only Api Gateway: Blocks all outsiders API Calls

            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
