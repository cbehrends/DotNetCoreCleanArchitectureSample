using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;

namespace Payments.Application.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}