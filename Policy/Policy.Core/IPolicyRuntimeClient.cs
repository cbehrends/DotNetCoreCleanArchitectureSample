using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Policy.Core.Model;

namespace Policy.Core
{
    /// <summary>
    ///     Interface for Policy client
    /// </summary>
    public interface IPolicyRuntimeClient
    {
        /// <summary>
        ///     Evaluates the policy for a given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user);

        /// <summary>
        ///     Determines whether the user has a permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);

        /// <summary>
        ///     Determines whether the user is in a role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role);

        /// <summary>
        ///     Gets a IEnumerable of strings for all policies applied to the given ClaimsPrincipal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetAppliedPolicyAsync(ClaimsPrincipal user);
    }
}