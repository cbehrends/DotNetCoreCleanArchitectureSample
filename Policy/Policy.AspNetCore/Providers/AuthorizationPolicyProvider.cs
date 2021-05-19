using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Policy.Core;

namespace Policy.AspNetCore.Providers
{
    /// <summary>
    ///     Authorization policy provider to automatically turn all permissions of a user into a ASP.NET Core authorization
    ///     policy
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider" />
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationPolicyProvider" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        /// <summary>
        ///     Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given
        ///     <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>
        ///     The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
        /// </returns>
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // check static policies first
            var policy = await base.GetPolicyAsync(policyName) ?? new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
            return policy;
        }
    }

    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPolicyRuntimeClient _client;

        public PermissionHandler(IPolicyRuntimeClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (await _client.HasPermissionAsync(context.User, requirement.Name)) context.Succeed(requirement);
        }
    }
}