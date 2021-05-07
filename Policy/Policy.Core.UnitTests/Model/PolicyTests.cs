using System;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework;
using Policy.Core.Model;
using Shouldly;

namespace Policy.Core.UnitTests.Model
{
    public class PolicyTests
    {
        Policy.Core.Model.Policy _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new Policy.Core.Model.Policy();
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
                                        new Role {Name = "c"},
                                        new Role {Name = "a"},
                                        new Role {Name = "b"}
                                    });

            ClaimsPrincipal user = TestUser.CreateWithPrefUserName("1", new[] { "c", "a" });

            PolicyResult policyResult = await _subject.EvaluateAsync(user);

            policyResult.Roles.ShouldBe(new[] { "a", "c" }, true);
        }

        [Test]
        public async Task Evaluate_Should_Not_Return_Unmatched_Roles()
        {
            _subject.Roles.AddRange(new[]
                                    {
                                        new Role {Name = "c"},
                                        new Role {Name = "a"},
                                        new Role {Name = "b"}
                                    });

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "foo" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Roles.ShouldBeEmpty();
        }

        [Test]
        public async Task Evaluate_Should_Return_Remove_Duplicate_Roles()
        {
            _subject.Roles.AddRange(new[]
                                    {
                                        new Role {Name = "a"},
                                        new Role {Name = "a"}
                                    });

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "a" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Roles.ShouldBe(new[] { "a" }, true);
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

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "foo" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBe(new[] { "a", "c" }, true);
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

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "foo" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

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

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "foo" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBe(new[] { "a" });
        }

        [Test]
        public async Task Evaluate_Should_Not_Allow_Identity_Roles_To_Match_Permissions()
        {
            _subject.Permissions.AddRange(new[]
                                          {
                                              new Permission {Name = "perm", Roles = {"role"}}
                                          });

            ClaimsPrincipal claimsPrincipal = TestUser.CreateWithPrefUserName("1", new[] { "role" });

            PolicyResult policyResult = await _subject.EvaluateAsync(claimsPrincipal);

            policyResult.Permissions.ShouldBeEmpty();
        }
    }
}