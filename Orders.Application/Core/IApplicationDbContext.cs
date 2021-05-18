using System.Threading;
using System.Threading.Tasks;
using Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Orders.Application.Core
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<RenderedService> RenderedServices { get; set; }

        DbSet<Service> Services { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}