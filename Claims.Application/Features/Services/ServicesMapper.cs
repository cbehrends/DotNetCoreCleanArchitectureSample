using AutoMapper;
using Claims.Application.Features.Services.Commands;
using Claims.Domain.Entities;

namespace Claims.Application.Features.Services
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