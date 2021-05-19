using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Policy.AspNetCore.Providers;
using Policy.Core;

namespace Policy.AspNetCore
{
    /// <summary>
    ///     Helper class to configure DI
    /// </summary>
    public static class InjectDependencies
    {
        /// <summary>
        ///     Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddPolicyClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient<IPolicyRuntimeClient, PolicyRuntimeClient>();
            services.Configure<Core.Model.Policy>(configuration);
            services.TryAddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<Core.Model.Policy>>().Value);
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionHandler>();
            return services;
        }
    }
}