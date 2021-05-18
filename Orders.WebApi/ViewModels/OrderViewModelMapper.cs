using AutoMapper;
using Orders.Domain.Entities;

namespace Orders.WebApi.ViewModels
{
    public class OrderViewModelMapper : Profile
    {
        public OrderViewModelMapper()
        {
            CreateMap<Order, OrderViewModel>()
                .ReverseMap();

            CreateMap<RenderedService, RenderedServiceViewModel>()
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Service.Description))
                .ForMember(d => d.Cost, opt => opt.MapFrom(s => s.Service.Cost))
                .ReverseMap();
        }
    }
}