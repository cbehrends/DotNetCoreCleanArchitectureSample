using System;
using System.Threading.Tasks;
using Claims.Application.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Features.Messaging
{
    public interface IGetClaim
    {
        int Id { get; set; }
    }
        
    public class GetClaimMessage: IGetClaim
    {
        public int Id { get; set; }
    }
    
    public class GetClaimMessageHandler: IConsumer<IGetClaim>
    {
       
        private readonly IApplicationDbContext _context;
        
        
        public GetClaimMessageHandler(IApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(IApplicationDbContext));
        }
        
        public Task Consume(ConsumeContext<IGetClaim> context)
        {
            return  context.RespondAsync(_context.Claims.SingleAsync(c => c.Id == context.Message.Id));
        }
    }
}