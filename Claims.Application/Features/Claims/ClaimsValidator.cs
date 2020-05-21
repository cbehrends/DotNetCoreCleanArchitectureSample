

using Claims.Domain.Entities;
using FluentValidation;

namespace Claims.Application.Features.Claims
{
    public class ClaimsValidator: AbstractValidator<Claim>
    {
        public ClaimsValidator()
        {
            RuleFor(claim => claim.FirstName).NotEmpty();
            RuleFor(claim => claim.FirstName).MaximumLength(50);
        }
    }
}