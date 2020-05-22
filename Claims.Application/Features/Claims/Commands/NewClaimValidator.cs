using System.Data;
using FluentValidation;

namespace Claims.Application.Features.Claims.Commands
{
    public class NewClaimValidator : AbstractValidator<NewClaim.Command>
    {
        public NewClaimValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
            RuleFor(claim => claim.ServicesRendered).NotNull().NotEmpty();
        }
    }
}