using FluentValidation;

namespace Claims.Application.Features.Services.Commands
{
    public class NewServiceValidator : AbstractValidator<NewService.Command>
    {
        public NewServiceValidator()
        {
            RuleFor(svc => svc.Description).NotEmpty();
            RuleFor(svc => svc.Description).MaximumLength(50);
        }
    }
}