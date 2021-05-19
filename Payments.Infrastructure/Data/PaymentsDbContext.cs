using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Core.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Data
{
    public class PaymentsDbContext : DbContext, IApplicationDbContext
    {
        public PaymentsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}