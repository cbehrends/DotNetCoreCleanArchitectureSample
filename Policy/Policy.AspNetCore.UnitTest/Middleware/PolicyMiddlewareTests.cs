using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Policy.AspNetCore.Middleware;
using Policy.Core;
using Policy.Core.Model;

namespace Policy.AspNetCore.UnitTests.Middleware
{
    public class PolicyMiddlewareTests
    {
        private PolicyMiddleware m_oPolicyMiddleware;
        private IPolicyRuntimeClient m_oPolicyRuntimeClient;
        private RequestDelegate m_oRequestDelegate;

        [SetUp]
        public void Setup()
        {
            m_oPolicyRuntimeClient = A.Fake<IPolicyRuntimeClient>();
            m_oRequestDelegate = delegate(HttpContext context)
            {
                Console.WriteLine(context.Connection.Id);
                return Task.CompletedTask;
            };
            m_oPolicyMiddleware = new PolicyMiddleware(m_oRequestDelegate);
        }

        [Test]
        public async Task Should_Pass_Through_If_User_Not_Authenticated()
        {
            var oContext = new DefaultHttpContext();

            await m_oPolicyMiddleware.InvokeAsync(oContext, m_oPolicyRuntimeClient);
            A.CallTo(() => m_oPolicyRuntimeClient.EvaluateAsync(A<ClaimsPrincipal>.Ignored))
                .MustNotHaveHappened();
        }

        [Test]
        public async Task Should_Fetch_Permissions_If_User_Is_Authenticated()
        {
            var oHttpContext = A.Fake<HttpContext>();
            var oClaimsPrincipal = A.Fake<ClaimsPrincipal>();
            var oClaimsIdentity = A.Fake<ClaimsIdentity>();
            A.CallTo(() => oClaimsIdentity.IsAuthenticated).Returns(true);

            var colClaims = new List<Claim>
            {
                new("preferred_username", "Han_Solo"),
                new("role", "WorkQueueFull"),
                new("iss", "Rebel_Alliance")
            };
            A.CallTo(() => oClaimsPrincipal.Claims).Returns(colClaims);
            A.CallTo(() => oClaimsPrincipal.Identity).Returns(oClaimsIdentity);
            A.CallTo(() => oHttpContext.User).Returns(oClaimsPrincipal);
            var oRequest = A.Fake<HttpRequest>();
            A.CallTo(() => oRequest.Path).Returns(new PathString("/api/site/42/bla/bla"));
            A.CallTo(() => oHttpContext.Request).Returns(oRequest);
            A.CallTo(() => m_oPolicyRuntimeClient.EvaluateAsync(A<ClaimsPrincipal>.Ignored))
                .Returns(new PolicyResult
                {
                    Roles = new[] {"WorkQueueFull"},
                    Permissions = new[] {"Ride_Solo"}
                });

            await m_oPolicyMiddleware.InvokeAsync(oHttpContext, m_oPolicyRuntimeClient);
        }
    }
}