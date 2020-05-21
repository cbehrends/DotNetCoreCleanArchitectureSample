using System.Linq;
using Claims.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Claims.Infrastructure.Identity
{
    public static class IdentityExtension
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}