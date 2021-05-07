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
            IPolicyRuntimeClient oFakePolicyClient = A.Fake<IPolicyRuntimeClient>();
            A.CallTo(()=> oFakePolicyClient.HasPermissionAsync(A<ClaimsPrincipal>.Ignored, A<string>.Ignored))
             .Returns(true);
            
            AuthorizationHandlerContext oFakeContext = A.Fake<AuthorizationHandlerContext>();
            List<PermissionRequirement> colPermissionRequirements = new() {new PermissionRequirement("MY_PERMISSION_REQUIREMENT")};
            A.CallTo(() => oFakeContext.Requirements).Returns(colPermissionRequirements);
            PermissionHandler oSystemUnderTest = new(oFakePolicyClient);
            
            oSystemUnderTest.HandleAsync(oFakeContext);
            
            A.CallTo(() => oFakeContext.Succeed(A<PermissionRequirement>.Ignored))
             .MustHaveHappened();
            
        }
        
        [Test]
        public void Succeed_Should_Not_Get_Called_If_User_Has_No_Permission()
        {
            IPolicyRuntimeClient oFakePolicyClient = A.Dummy<IPolicyRuntimeClient>();
            AuthorizationHandlerContext oFakeContext = A.Fake<AuthorizationHandlerContext>();
            PermissionHandler oSystemUnderTest = new(oFakePolicyClient);
            
            oSystemUnderTest.HandleAsync(new AuthorizationHandlerContext(new List<PermissionRequirement>(), null, null));
            
            A.CallTo(() => oFakeContext.Succeed(A<IAuthorizationRequirement>.Ignored))
             .MustNotHaveHappened();
        }
    }
}