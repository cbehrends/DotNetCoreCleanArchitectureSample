using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Behaviours;
using FluentValidation;
using MediatR;
using NUnit.Framework;
using ValidationException = Common.ApplicationCore.Exceptions.ValidationException;

namespace Common.UnitTests.Behaviors
{
    public class ValidationTests
    {
        [Test]
        public async Task Should_Validate_And_Throw_ValidationException()
        {
            var mockReq = new MockRequest();

            //public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            IValidator<IRequest<MockRequest>> val = new InlineValidator<IRequest<MockRequest>>();

            var validators = new List<IValidator<IRequest<MockRequest>>> {val};

            var validationBehavior = new ValidationBehavior<MockRequest, Unit>(new List<MockRequestValidator>
            {
                new()
            });
            var cmd = new MockRequest
            {
                Name = "" // Should fail
            };

            var cmdHandler = new MockRequestHandler();

            Assert.ThrowsAsync<ValidationException>(() =>
                validationBehavior.Handle(cmd, CancellationToken.None,
                    () => cmdHandler.Handle(cmd, CancellationToken.None))
            );
        }

        [Test]
        public async Task Should_Validate_Success()
        {
            var mockReq = new MockRequest();

            //public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            IValidator<IRequest<MockRequest>> val = new InlineValidator<IRequest<MockRequest>>();

            var validators = new List<IValidator<IRequest<MockRequest>>> {val};

            var validationBehavior = new ValidationBehavior<MockRequest, Unit>(new List<MockRequestValidator>
            {
                new()
            });
            var cmd = new MockRequest
            {
                Name = "Hank Hill"
            };

            var cmdHandler = new MockRequestHandler();

            Assert.DoesNotThrowAsync(() =>
                validationBehavior.Handle(cmd, CancellationToken.None,
                    () => cmdHandler.Handle(cmd, CancellationToken.None)));
        }

        [Test]
        public async Task Should_Not_Validate_If_No_Validaiton_Exists()
        {
            var validationBehavior = new ValidationBehavior<MockUnValidatedRequest, Unit>(null);

            var cmd = new MockUnValidatedRequest();

            var cmdHandler = new MockUnValidatedRequestHandler();

            Assert.DoesNotThrowAsync(() =>
                validationBehavior.Handle(cmd, CancellationToken.None,
                    () => cmdHandler.Handle(cmd, CancellationToken.None)));
        }
    }
}