using FluentValidation;

namespace Orders.Application.Features.Orders.Commands
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrder.Command>
    {
        public UpdateOrderValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
            RuleFor(claim => claim.ServicesRendered).NotNull().NotEmpty();
        }
    }
}