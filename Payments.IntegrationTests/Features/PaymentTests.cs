using System;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Features.Payments;

namespace Payments.IntegrationTests.Features
{
    using static TestFixture;
    public class PaymentTests: TestBase
    {

        [Test]
        public void Apply_Payment_Should_Fail_On_Validation_Error()
        {
            var command = new ApplyPayment.Command(); // Should fail validation
            
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public async Task Apply_Payment_Should_Succeed_When_Valid()
        {
            var command = new ApplyPayment.Command
            {
                ClaimId = 1,
                PaymentAmount = 200
            };

            var newPayment = await SendAsync(command);
            newPayment.Id.Should().BeGreaterThan(-1);
            newPayment.ClaimId.Should().Be(command.ClaimId);
            newPayment.PaymentAmount.Should().Be(command.PaymentAmount);
            newPayment.PaymentDate.Should().BeBefore(DateTimeOffset.Now.AddMinutes(1));

        }
        
        [Test]
        public async Task Should_Get_Payment_By_Id()
        {
            var command = new ApplyPayment.Command
            {
                ClaimId = 1,
                PaymentAmount = 200
            };
            
            
            var newPayment = await SendAsync(command);

            var payment = await SendAsync(new GetPaymentById.Query {Id = newPayment.Id});

            payment.Should().NotBeNull();
            
        }
    }
}