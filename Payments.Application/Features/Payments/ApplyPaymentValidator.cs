using FluentValidation;

namespace Payments.Application.Features.Payments
{
    public class ApplyPaymentValidator: AbstractValidator<ApplyPayment.Command>
    {
        public ApplyPaymentValidator()
        {
            RuleFor(pay => pay.OrderId).GreaterThan(0);
            RuleFor(pay => pay.PaymentAmount).GreaterThan(0);

        }
    }
}