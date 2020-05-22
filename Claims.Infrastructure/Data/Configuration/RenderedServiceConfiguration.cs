using Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Claims.Infrastructure.Data.Configuration
{
    public class RenderedServiceConfiguration: IEntityTypeConfiguration<RenderedService>
    {
        public void Configure(EntityTypeBuilder<RenderedService> builder)
        {
            builder
                .HasKey(svc => svc.Id);

            builder
                .HasOne(svc => svc.Service);
        }
    }
}