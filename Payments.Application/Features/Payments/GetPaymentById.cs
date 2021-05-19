using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ApplicationCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Core.Interfaces;
using Payments.Domain.Entities;

namespace Payments.Application.Features.Payments
{
    public static class GetPaymentById
    {
        public class Query : IRequest<Payment>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Payment>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
            }

            public async Task<Payment> Handle(Query request, CancellationToken cancellationToken)
            {
                var retVal = await _context
                    .Payments
                    .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if (retVal == null) throw new NotFoundException($"Payment with Id {request.Id} not found");

                return retVal;
            }
        }
    }
}