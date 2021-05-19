using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Policy.Core.Model;
using Shouldly;

namespace Policy.Core.UnitTests.Model
{
    public class PolicyTests
    {
        private Core.Model.Policy _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new Core.Model.Policy();
        }

        [Test]
        public void Evaluate_Should_Require_User()
        {
            Func<Task> funcTest = () => _subject.EvaluateAsync(null);
            funcTest.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public async Task Evaluate_Should_Return_Matched_Roles()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "c", Description = "C"},
                new Role {Name = "a", Description = "A"},
                new Role {Name = "b", Description =  "B"}
            });

            var user = TestUser.CreateWithPrefUserName("1", new[] {"c", "a"});

            var policyResult = await _subject.EvaluateAsync(user);

            policyResult.Roles.ShouldBe(new[] {"a", "c"}, true);
        }

        [Test]
        public async Task Evaluate_Should_Not_Return_Unmatched_Roles()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "c", Description = "C"},
                new Role {Name = "a", Description = "A"},
                new Role {Name = "b", Description =  "B"}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"foo"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Roles.ShouldBeEmpty();
        }

        [Test]
        public async Task Evaluate_Should_Remove_Duplicate_Roles()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "a", Description = "A"},
                new Role {Name = "a", Description = "A"}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"a"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Roles.ShouldBe(new[] {"a"}, true);
        }

        [Test]
        public async Task Evaluate_Should_Return_Matched_Permissions()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "foo"},
                new Role {Name = "xoxo"}
            });
            _subject.Permissions.AddRange(new[]
            {
                new Permission {Name = "a", Roles = {"foo"}},
                new Permission {Name = "c", Roles = {"foo"}},
                new Permission {Name = "b", Roles = {"xoxo"}}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"foo"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBe(new[] {"a", "c"}, true);
        }

        [Test]
        public async Task Evaluate_Should_Not_Return_Unmatched_Permissions()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "role"}
            });
            _subject.Permissions.AddRange(new[]
            {
                new Permission {Name = "a", Roles = {"xoxo"}},
                new Permission {Name = "c", Roles = {"xoxo"}},
                new Permission {Name = "b", Roles = {"xoxo"}}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"foo"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBeEmpty();
        }

        [Test]
        public async Task Evaluate_Should_Remove_Duplicate_Permissions()
        {
            _subject.Roles.AddRange(new[]
            {
                new Role {Name = "foo"}
            });
            _subject.Permissions.AddRange(new[]
            {
                new Permission {Name = "a", Roles = {"foo"}},
                new Permission {Name = "a", Roles = {"foo"}}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"foo"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBe(new[] {"a"});
        }

        [Test]
        public async Task Evaluate_Should_Not_Allow_Identity_Roles_To_Match_Permissions()
        {
            _subject.Permissions.AddRange(new[]
            {
                new Permission {Name = "perm", Roles = {"role"}}
            });

            var claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] {"role"});

            var policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBeEmpty();
        }
    }
}