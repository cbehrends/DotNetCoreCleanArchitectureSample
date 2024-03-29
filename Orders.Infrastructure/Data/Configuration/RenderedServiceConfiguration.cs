using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data.Configuration
{
    public class RenderedServiceConfiguration : IEntityTypeConfiguration<RenderedService>
    {
        public void Configure(EntityTypeBuilder<RenderedService> builder)
        {
            builder
                .HasKey(svc => svc.Id);

            builder
                .HasOne(svc => svc.Service);

            builder.Property(svc => svc.ServiceId)
                .IsRequired();

            builder.Property(svc => svc.Cost)
                .HasColumnType("decimal(7,2)");

            builder.HasOne(svc => svc.Service)
                .WithMany()
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}