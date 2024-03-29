using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data.Configuration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder
                .HasKey(service => service.Id);

            builder
                .Property(service => service.Description)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(service => service.Cost)
                .HasColumnType("decimal(7,2)");

            builder
                .HasIndex(svc => svc.Description)
                .IsUnique();
        }
    }
}