using AutoMapper;
using Orders.Application.Features.Orders.Commands;
using Orders.Application.Features.Orders.Model;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Orders
{
    public class OrdersMapper : Profile
    {
        public OrdersMapper()
        {
            CreateMap<Order, NewOrder.Command>();
            CreateMap<NewOrder.Command, Order>()
                .ForMember(order => order.Id, opt => opt.Ignore())
                .ForMember(order => order.AmountDue, opt => opt.Ignore());

            CreateMap<RenderedService, RenderedServiceDto>()
                .ForMember(d => d.ServiceId, opt => opt.MapFrom(s => s.ServiceId))
                .ForMember(d => d.OrderId, opt => opt.MapFrom(s => s.ClaimId))
                .ReverseMap();
        }
    }
}