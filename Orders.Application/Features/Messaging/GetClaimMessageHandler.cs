using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Core;

namespace Orders.Application.Features.Messaging
{
    public interface IGetOrder
    {
        int Id { get; set; }
    }

    public class GetOrderMessage : IGetOrder
    {
        public int Id { get; set; }
    }

    public class GetClaimMessageHandler : IConsumer<IGetOrder>
    {
        private readonly IApplicationDbContext _context;


        public GetClaimMessageHandler(IApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
        }

        public Task Consume(ConsumeContext<IGetOrder> context)
        {
            return context.RespondAsync(_context.Orders.SingleAsync(c => c.Id == context.Message.Id));
        }
    }
}