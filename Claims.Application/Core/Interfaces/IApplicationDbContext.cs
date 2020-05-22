using System.Threading;
using System.Threading.Tasks;
using Claims.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Claim> Claims { get; set; }

        DbSet<Service> Services { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}