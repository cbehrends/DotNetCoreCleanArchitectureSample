using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework;
using Policy.Core.Model;
using Shouldly;

namespace Policy.Core.UnitTests
{
    public class PolicyServerRuntimeClientTests 
    {
        private IPolicyRuntimeClient m_oPolicyClient;
        private ClaimsPrincipal m_oValidClaimsPrincipal;

        [SetUp]
        public void Setup()
        {
            List<Claim> colClaims = new();
            Claim oClaim = new("role", "Nerf_Herders");
            colClaims.Add(oClaim);
            ClaimsIdentity oClaimsIdentity = new(colClaims);
            Policy.Core.Model.Policy oNerfHerders = new();
            oNerfHerders.Roles.Add(new Role { Name = "Nerf_Herders" });
            Permission oSoloHerder = new() { Name = "SoloNerfHerder" };
            Permission oNewbHerder = new() { Name = "NoobNerfHerder" };

            oSoloHerder.Roles.AddRange(oNerfHerders.Roles.Select(role => role.Name));
            oNewbHerder.Roles.AddRange(oNerfHerders.Roles.Select(role => role.Name));

            oNerfHerders.Permissions.Add(oSoloHerder);
            oNerfHerders.Permissions.Add(oNewbHerder);

            m_oValidClaimsPrincipal = new ClaimsPrincipal(oClaimsIdentity);
            m_oPolicyClient = new PolicyRuntimeClient(oNerfHerders);
        }

        [TestFixture]
        public class IsInRoleAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_True_If_User_In_Given_Role()
            {
                bool bResult = await m_oPolicyClient.IsInRoleAsync(m_oValidClaimsPrincipal, "Nerf_Herders");
                bResult.ShouldBeTrue();
            }

            [Test]
            public async Task Should_Return_False_If_User_Is_Not_In_Given_Role()
            {
                bool bResult = await m_oPolicyClient.IsInRoleAsync(m_oValidClaimsPrincipal, "Bounty_Hunters");
                bResult.ShouldBeFalse();
            }

            [Test]
            public async Task Should_Throw_ArgumentNullException_If_User_Is_Null()
            {
                await m_oPolicyClient.IsInRoleAsync(null, "Don't Matter").ShouldThrowAsync<ArgumentNullException>();
            }
        }

        [TestFixture]
        public class HasPermissionAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_True_If_User_Has_Permission()
            {
                bool bHasPermission = await m_oPolicyClient.HasPermissionAsync(m_oValidClaimsPrincipal, "SoloNerfHerder");
                bHasPermission.ShouldBeTrue();
            }

            [Test]
            public async Task Should_Return_False_If_User_Does_Not_Have_Permission()
            {
                bool bHasPermission = await m_oPolicyClient.HasPermissionAsync(m_oValidClaimsPrincipal, "BanthaRider");
                bHasPermission.ShouldBeFalse();
            }
        }
        
        [TestFixture]
        public class GetAppliedPolicyAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_Applied_Policies()
            {
                IEnumerable<string> colAppliedPolicy = await m_oPolicyClient.GetAppliedPolicyAsync(m_oValidClaimsPrincipal);
                colAppliedPolicy.ShouldNotBeEmpty();
            }
        }
    }
}