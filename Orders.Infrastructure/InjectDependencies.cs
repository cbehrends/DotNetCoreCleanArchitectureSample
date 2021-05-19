using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Core;
using Orders.Infrastructure.Data;

namespace Orders.Infrastructure
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddOrdersInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionString"],
                    b => b.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<OrdersDbContext>());

            return services;
        }
    }
}