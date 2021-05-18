using System.Reflection;
using Orders.Application.Core;
using Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Orders.Infrastructure.Data
{
    public class OrdersDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<RenderedService> RenderedServices { get; set; }
        public DbSet<Service> Services { get; set; }

        public OrdersDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}