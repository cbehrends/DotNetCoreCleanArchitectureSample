using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Data.Configuration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .HasKey(payment => payment.Id);

            builder
                .Property(payment => payment.PaymentAmount)
                .HasColumnType("decimal(7, 2)");
            
            builder
                .Property(payment => payment.OrderId)
                .IsRequired();
            
            builder
                .Property(payment => payment.PaymentDate)
                .IsRequired();

        }
    }
}