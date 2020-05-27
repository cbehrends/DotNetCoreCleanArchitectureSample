using FluentValidation;

namespace Common.UnitTests.Behaviors
{
    public class MockRequestValidator: AbstractValidator<MockRequest>
    {
        public MockRequestValidator()
        {
            RuleFor(mock => mock.Name).NotEmpty().MaximumLength(50);
        }
    }
}