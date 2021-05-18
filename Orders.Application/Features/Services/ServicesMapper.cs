using AutoMapper;
using Orders.Application.Features.Services.Commands;
using Orders.Domain.Entities;

namespace Orders.Application.Features.Services
{
    public class ServicesMapper : Profile
    {
        public ServicesMapper()
        {
            CreateMap<Service, NewService.Command>();

            CreateMap<NewService.Command, Service>()
                .ForMember(service => service.Id, opt => opt.Ignore());
        }
    }
}