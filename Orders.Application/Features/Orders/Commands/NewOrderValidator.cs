using FluentValidation;

namespace Orders.Application.Features.Orders.Commands
{
    public class NewOrderValidator : AbstractValidator<NewOrder.Command>
    {
        public NewOrderValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
            RuleFor(claim => claim.ServicesRendered).NotNull().NotEmpty();
        }
    }
}