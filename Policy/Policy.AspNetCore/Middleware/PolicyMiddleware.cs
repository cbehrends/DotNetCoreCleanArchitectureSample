using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Policy.Core;

namespace Policy.AspNetCore.Middleware
{
    /// <summary>
    ///     Middleware to automatically turn application roles and permissions into claims
    /// </summary>
    public class PolicyMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PolicyMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public PolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///     InvokeAsync
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IPolicyRuntimeClient client)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var policy = await client.EvaluateAsync(context.User);

                var roleClaims = policy.Roles.Select(x => new Claim("role", x));
                var permissionClaims = policy.Permissions.Select(x => new Claim("permission", x));

                var id = new ClaimsIdentity("PolicyMiddleware", "name", "role");
                id.AddClaims(roleClaims);
                id.AddClaims(permissionClaims);

                context.User.AddIdentity(id);
            }

            await _next(context);
        }
    }
}