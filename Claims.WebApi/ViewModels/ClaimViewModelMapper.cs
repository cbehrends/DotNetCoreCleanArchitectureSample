using AutoMapper;
using Claims.Domain.Entities;

namespace Claims.WebApi.ViewModels
{
    public class ClaimViewModelMapper: Profile
    {
        public ClaimViewModelMapper()
        {
            CreateMap<Claim, ClaimViewModel>()
                .ReverseMap();

            CreateMap<RenderedService, RenderedServiceViewModel>()
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Service.Description))
                .ReverseMap();
        }
        
    }
}