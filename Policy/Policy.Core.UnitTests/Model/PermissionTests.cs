using System;
using NUnit.Framework;
using Policy.Core.Model;
using Shouldly;

namespace Policy.Core.UnitTests.Model
{
    public class PermissionTests
    {
        private readonly Permission _subject;

        public PermissionTests()
        {
            _subject = new Permission();
        }

        [Test]
        public void Evaluate_Should_Require_Roles()
        {
            Action funcTest = () => _subject.Evaluate(null);
            funcTest.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Evaluate_Should_Fail_For_Invalid_Roles()
        {
            var result = _subject.Evaluate(new[] {"foo"});
            result.ShouldBeFalse();
        }

        [Test]
        public void Evaluate_Should_Succeed_For_Valid_Roles()
        {
            _subject.Roles.Add("foo");
            var result = _subject.Evaluate(new[] {"foo"});
            result.ShouldBeTrue();
        }
    }
}