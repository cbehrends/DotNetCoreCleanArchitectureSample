using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Core.Interfaces;
using Payments.Infrastructure.Data;

namespace Payments.Infrastructure
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PaymentsDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionString"],
                    b => b.MigrationsAssembly(typeof(PaymentsDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, PaymentsDbContext>();

            return services;
        }
    }
}