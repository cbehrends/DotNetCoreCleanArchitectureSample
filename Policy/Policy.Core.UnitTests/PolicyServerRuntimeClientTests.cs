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
        private IPolicyRuntimeClient _policyClient;
        private ClaimsPrincipal _validClaimsPrincipal;

        [SetUp]
        public void Setup()
        {
            List<Claim> claims = new();
            Claim claim = new("role", "Nerf_Herders");
            claims.Add(claim);
            ClaimsIdentity oClaimsIdentity = new(claims);
            Core.Model.Policy oNerfHerders = new();
            oNerfHerders.Roles.Add(new Role {Name = "Nerf_Herders"});
            Permission soloHerder = new() {Name = "SoloNerfHerder"};
            Permission newbHerder = new() {Name = "NoobNerfHerder"};

            soloHerder.Roles.AddRange(oNerfHerders.Roles.Select(role => role.Name));
            newbHerder.Roles.AddRange(oNerfHerders.Roles.Select(role => role.Name));

            oNerfHerders.Permissions.Add(soloHerder);
            oNerfHerders.Permissions.Add(newbHerder);

            _validClaimsPrincipal = new ClaimsPrincipal(oClaimsIdentity);
            _policyClient = new PolicyRuntimeClient(oNerfHerders);
        }

        [TestFixture]
        public class IsInRoleAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_True_If_User_In_Given_Role()
            {
                var bResult = await _policyClient.IsInRoleAsync(_validClaimsPrincipal, "Nerf_Herders");
                bResult.ShouldBeTrue();
            }

            [Test]
            public async Task Should_Return_False_If_User_Is_Not_In_Given_Role()
            {
                var bResult = await _policyClient.IsInRoleAsync(_validClaimsPrincipal, "Bounty_Hunters");
                bResult.ShouldBeFalse();
            }

            [Test]
            public async Task Should_Throw_ArgumentNullException_If_User_Is_Null()
            {
                await _policyClient.IsInRoleAsync(null, "Don't Matter").ShouldThrowAsync<ArgumentNullException>();
            }
        }

        [TestFixture]
        public class HasPermissionAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_True_If_User_Has_Permission()
            {
                var hasPermission =
                    await _policyClient.HasPermissionAsync(_validClaimsPrincipal, "SoloNerfHerder");
                hasPermission.ShouldBeTrue();
            }

            [Test]
            public async Task Should_Return_False_If_User_Does_Not_Have_Permission()
            {
                var hasPermission = await _policyClient.HasPermissionAsync(_validClaimsPrincipal, "BanthaRider");
                hasPermission.ShouldBeFalse();
            }
        }

        [TestFixture]
        public class GetAppliedPolicyAsync : PolicyServerRuntimeClientTests
        {
            [Test]
            public async Task Should_Return_Applied_Policies()
            {
                var colAppliedPolicy = await _policyClient.GetAppliedPolicyAsync(_validClaimsPrincipal);
                colAppliedPolicy.ShouldNotBeEmpty();
            }
        }
    }
}