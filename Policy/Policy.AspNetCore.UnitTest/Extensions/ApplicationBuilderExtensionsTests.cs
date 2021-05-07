using System;
using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using ApplicationBuilderExtensions = Policy.AspNetCore.Extensions.ApplicationBuilderExtensions;

namespace Policy.AspNetCore.UnitTests.Extensions
{
    public class ApplicationBuilderExtensionsTests
    {

        public class LoadMiddlewareTests: ApplicationBuilderExtensionsTests
        {
            [Test]
            public void Should_Load_PolicyMiddleware()
            {
                IApplicationBuilder oApplicationBuilderFake = A.Fake<IApplicationBuilder>();
                ApplicationBuilderExtensions.UsePolicyClaims(oApplicationBuilderFake);
                A.CallTo(() => oApplicationBuilderFake.Use(A<Func<RequestDelegate, RequestDelegate>>.Ignored))
                 .MustHaveHappened();
            }
        }
    }
}