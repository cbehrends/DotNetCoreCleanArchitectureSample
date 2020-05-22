using Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Claims.Infrastructure.Data.Configuration
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder
                .HasKey(claim => claim.Id);

            builder
                .Property(claim => claim.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(claim => claim.ServicesRendered)
                .WithOne(service => service.Claim)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}