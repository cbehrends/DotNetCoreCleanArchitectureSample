using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Policy.Core.Model
{
    /// <summary>
    /// Models a policy
    /// </summary>
    public class Policy
    {

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Role> Roles { get; } = new();

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permission> Permissions { get; } = new();

        internal Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string[] roles = Roles.Where(role => role.Evaluate(user)).Select(role => role.Name).ToArray();
            string[] permissions = Permissions.Where(permission => permission.Evaluate(roles)).Select(permission => permission.Name).ToArray();

            PolicyResult policyResult = new()
                                         {
                                                   Roles = roles.Distinct(),
                                                   Permissions = permissions.Distinct()
                                               };

            return Task.FromResult(policyResult);
        }
    }
}