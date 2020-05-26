using FluentValidation;

namespace Claims.Application.Features.Claims.Commands
{
    public class UpdateClaimValidator: AbstractValidator<UpdateClaim.Command>
    {
        public UpdateClaimValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
            RuleFor(claim => claim.ServicesRendered).NotNull().NotEmpty();
        }
    }
}