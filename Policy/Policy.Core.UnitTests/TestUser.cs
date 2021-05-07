using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Policy.Core.UnitTests
{
    public static class TestUser
    {
        public static ClaimsPrincipal CreateWithPrefUserName(string preferedUserName,
                                                    IEnumerable<string> roles = null, IEnumerable<Claim> claims = null)
        {
            List<Claim> list = new() { new Claim("prefered_username", preferedUserName) };

            if (roles != null) list.AddRange(roles.Select(x => new Claim("role", x)));

            if (claims != null) list.AddRange(claims);

            ClaimsIdentity ci = new(list, "pwd", "name", "role");
            ClaimsPrincipal cp = new(ci);

            return cp;
        }
    }
}