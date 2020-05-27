using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Data
{
    public class PaymentsDbContext: DbContext, IApplicationDbContext
    {
        public DbSet<Payment> Payments { get; set; }
        
        public PaymentsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
        
    }
}