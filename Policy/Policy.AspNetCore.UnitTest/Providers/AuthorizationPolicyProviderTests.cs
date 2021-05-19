using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Policy.AspNetCore.Providers;
using Shouldly;

namespace Policy.AspNetCore.UnitTests.Providers
{
    public class AuthorizationPolicyProviderTests
    {
        [Test]
        public async Task GetPolicyAsync_Should_Return_AuthorizationPolicy_If_Found()
        {
            List<PermissionRequirement> colPermissionRequirements =
                new() {new PermissionRequirement("MY_PERMISSION_REQUIREMENT")};
            List<string> colAuthSchemes = new() {"Default"};
            AuthorizationPolicy policy = new(colPermissionRequirements, colAuthSchemes);

            var oOptionsFactory = A.Fake<IOptionsFactory<AuthorizationOptions>>();
            AuthorizationOptions oOptions = new()
            {
                DefaultPolicy = new AuthorizationPolicy(colPermissionRequirements, colAuthSchemes),
                FallbackPolicy = new AuthorizationPolicy(colPermissionRequirements, colAuthSchemes),
                InvokeHandlersAfterFailure = false
            };
            oOptions.AddPolicy("TASKING", policy);
            A.CallTo(() => oOptionsFactory.Create(A<string>.Ignored))
                .Returns(oOptions);

            AuthorizationPolicyProvider oSystemUnderTest =
                new(new OptionsManager<AuthorizationOptions>(oOptionsFactory));

            var oAuthorizationPolicy = await oSystemUnderTest.GetPolicyAsync("TASKING");

            oAuthorizationPolicy.ShouldNotBeNull();
            ((PermissionRequirement) oAuthorizationPolicy.Requirements[0]).Name.ShouldBe("MY_PERMISSION_REQUIREMENT");
        }

        [Test]
        public async Task Should_Create_New_DefaultPolicyProvider_With_Given_Options()
        {
            var oOptions = A.Dummy<IOptions<AuthorizationOptions>>();

            AuthorizationPolicyProvider oSystemUnderTest = new(oOptions);
            oSystemUnderTest.ShouldNotBeNull();
            var oPolicy = await oSystemUnderTest.GetDefaultPolicyAsync();
            oPolicy.ShouldNotBeNull();
        }
    }
}