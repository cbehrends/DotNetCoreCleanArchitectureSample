using System;
using NUnit.Framework;
using Policy.Core.Model;
using Shouldly;

namespace Policy.Core.UnitTests.Model
{
    public class RoleTests
    {
        private readonly Role m_oRole;

        protected RoleTests()
        {
            m_oRole = new Role
            {
                Name = "foo",
                Description = "Foo"
            };
        }

        [TestFixture]
        public class Evaluate : RoleTests
        {
            [Test]
            public void Evaluate_Should_Require_User()
            {
                Action funcTest = () => m_oRole.Evaluate(null);
                funcTest.ShouldThrow<ArgumentNullException>();
            }

            [Test]
            public void Evaluate_Should_Fail_For_Invalid_Role()
            {
                var claimsPrincipal = TestUser.CreateWithPrefUserName("hank_hill");
                var result = m_oRole.Evaluate(claimsPrincipal);

                result.ShouldBeFalse();
            }

            [Test]
            public void Evaluate_Should_Succeed_For_Valid_Role()
            {
                var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"foo"});
                var result = m_oRole.Evaluate(claimsPrincipal);
                result.ShouldBeTrue();
            }
        }
    }
}