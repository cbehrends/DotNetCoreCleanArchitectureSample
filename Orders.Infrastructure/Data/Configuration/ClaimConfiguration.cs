using Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orders.Infrastructure.Data.Configuration
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(claim => claim.Id);

            builder
                .Property(claim => claim.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(claim => claim.AmountDue)
                .HasColumnType("decimal(8,2)");
            
            builder.Property(claim => claim.TotalAmount)
                .HasColumnType("decimal(8,2)");

            builder.HasMany(claim => claim.ServicesRendered)
                .WithOne(service => service.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}