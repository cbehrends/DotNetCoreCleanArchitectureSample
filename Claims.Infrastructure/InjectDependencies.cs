using Claims.Application.Core.Interfaces;
using Claims.Infrastructure.Data;
using Claims.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Infrastructure
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddClaimsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClaimsDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ClaimsDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ClaimsDbContext>());

            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<ClaimsDbContext>();
            
            // services.AddIdentityServer()
            //     .AddApiAuthorization<AppUser, ClaimsDbContext>();

            services.AddTransient<IIdentityService, IdentityService>();


            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}