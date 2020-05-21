using Claims.Domain.Entities;
using FluentValidation;

namespace Claims.Application.Features.Claims.Commands
{
    public class NewClaimValidator: AbstractValidator<NewClaim.Command>
    {
        public NewClaimValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
        }
    }
}