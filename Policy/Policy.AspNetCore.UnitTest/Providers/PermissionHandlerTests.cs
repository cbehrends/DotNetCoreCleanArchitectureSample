using System.Collections.Generic;
using System.Security.Claims;
using FakeItEasy;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using Policy.AspNetCore.Providers;
using Policy.Core;

namespace Policy.AspNetCore.UnitTests.Providers
{
    public class PermissionHandlerTests
    {
        [Test]
        public void Succeed_Should_Get_Called_If_User_Has_Permission()
        {
            var fakePolicyClient = A.Fake<IPolicyRuntimeClient>();
            A.CallTo(() => fakePolicyClient.HasPermissionAsync(A<ClaimsPrincipal>.Ignored, A<string>.Ignored))
                .Returns(true);

            var fakeContext = A.Fake<AuthorizationHandlerContext>();
            List<PermissionRequirement> requirements =
                new() {new PermissionRequirement("MY_PERMISSION_REQUIREMENT")};
            A.CallTo(() => fakeContext.Requirements).Returns(requirements);
            PermissionHandler systemUnderTest = new(fakePolicyClient);

            systemUnderTest.HandleAsync(fakeContext);

            A.CallTo(() => fakeContext.Succeed(A<PermissionRequirement>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public void Succeed_Should_Not_Get_Called_If_User_Has_No_Permission()
        {
            var fakePolicyClient = A.Dummy<IPolicyRuntimeClient>();
            var fakeContext = A.Fake<AuthorizationHandlerContext>();
            PermissionHandler systemUnderTest = new(fakePolicyClient);

            systemUnderTest.HandleAsync(new AuthorizationHandlerContext(new List<PermissionRequirement>(), null,
                null));

            A.CallTo(() => fakeContext.Succeed(A<IAuthorizationRequirement>.Ignored))
                .MustNotHaveHappened();
        }
    }
}