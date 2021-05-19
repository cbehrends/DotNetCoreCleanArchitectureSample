using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Policy.Core.Model;

namespace Policy.Core
{
    /// <summary>
    ///     Policy client
    /// </summary>
    public sealed class PolicyRuntimeClient : IPolicyRuntimeClient
    {
        private readonly Model.Policy _policy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PolicyRuntimeClient" /> class.
        /// </summary>
        /// <param name="policy">The Policy</param>
        public PolicyRuntimeClient(Model.Policy policy)
        {
            _policy = policy;
        }

        /// <summary>
        ///     Determines whether the user is in a role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var policyResult = await EvaluateAsync(user);
            return policyResult.Roles.Contains(role);
        }

        /// <summary>
        ///     Gets a IEnumerable of strings for all policies applied to the given ClaimsPrincipal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAppliedPolicyAsync(ClaimsPrincipal user)
        {
            var policyResult = await EvaluateAsync(user);
            return policyResult.Permissions;
        }


        /// <summary>
        ///     Determines whether the user has a permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var policyResult = await EvaluateAsync(user);
            return policyResult.Permissions.Contains(permission);
        }

        /// <summary>
        ///     Evaluates the oPolicy for a given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return _policy.EvaluateAsync(user);
        }
    }
}