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
            List<PermissionRequirement> permissionRequirements =
                new() {new PermissionRequirement("MY_PERMISSION_REQUIREMENT")};
            List<string> authSchemes = new() {"Default"};
            AuthorizationPolicy policy = new(permissionRequirements, authSchemes);

            var optionsFactory = A.Fake<IOptionsFactory<AuthorizationOptions>>();
            AuthorizationOptions options = new()
            {
                DefaultPolicy = new AuthorizationPolicy(permissionRequirements, authSchemes),
                FallbackPolicy = new AuthorizationPolicy(permissionRequirements, authSchemes),
                InvokeHandlersAfterFailure = false
            };
            options.AddPolicy("TASKING", policy);
            A.CallTo(() => optionsFactory.Create(A<string>.Ignored))
                .Returns(options);

            AuthorizationPolicyProvider systemUnderTest =
                new(new OptionsManager<AuthorizationOptions>(optionsFactory));

            var authorizationPolicy = await systemUnderTest.GetPolicyAsync("TASKING");

            authorizationPolicy.ShouldNotBeNull();
            ((PermissionRequirement) authorizationPolicy.Requirements[0]).Name.ShouldBe("MY_PERMISSION_REQUIREMENT");
        }

        [Test]
        public async Task Should_Create_New_DefaultPolicyProvider_With_Given_Options()
        {
            var options = A.Dummy<IOptions<AuthorizationOptions>>();

            AuthorizationPolicyProvider systemUnderTest = new(options);
            systemUnderTest.ShouldNotBeNull();
            var policy = await systemUnderTest.GetDefaultPolicyAsync();
            policy.ShouldNotBeNull();
        }
    }
}