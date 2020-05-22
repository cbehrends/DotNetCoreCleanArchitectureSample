using System.Reflection;
using Claims.Application.Core.Interfaces;
using Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Data
{
    public class ClaimsDbContext: DbContext, IApplicationDbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<RenderedService> RenderedServices { get; set; }
        public DbSet<Service> Services { get; set; }

        public ClaimsDbContext(DbContextOptions options): base(options){}
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

    }
}