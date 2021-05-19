using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data
{
    public class OrdersDbContext : DbContext, IApplicationDbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<RenderedService> RenderedServices { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}