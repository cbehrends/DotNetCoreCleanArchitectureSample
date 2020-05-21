using AutoMapper;
using Claims.Application.Features.Services.Commands;
using Claims.Domain.Entities;

namespace Claims.Application.Features.Services
{
    public class ServiceMapper: Profile
    {
        public ServiceMapper()
        {
            CreateMap<Service, NewService.Command>();
            CreateMap<NewService.Command, Service>()
                .ForMember(service => service.Id, opt => opt.Ignore());
        }
    }
}