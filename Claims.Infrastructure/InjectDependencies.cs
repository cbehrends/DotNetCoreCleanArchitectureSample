using Claims.Application.Core.Interfaces;
using Claims.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Infrastructure
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddClaimsInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ClaimsDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ClaimsDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ClaimsDbContext>());

            return services;
        }
    }
}