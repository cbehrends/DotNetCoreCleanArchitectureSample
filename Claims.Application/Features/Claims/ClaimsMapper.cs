using AutoMapper;
using Claims.Application.Features.Claims.Commands;
using Claims.Application.Features.Claims.Model;
using Claims.Domain.Entities;

namespace Claims.Application.Features.Claims
{
    public class ClaimsMapper : Profile
    {
        public ClaimsMapper()
        {
            CreateMap<Claim, NewClaim.Command>();
            CreateMap<NewClaim.Command, Claim>()
                .ForMember(claim => claim.Id, opt => opt.Ignore())
                .ForMember(claim => claim.AmountDue, opt => opt.Ignore());

            CreateMap<RenderedService, RenderedServiceDto>()
                .ForMember(d => d.ServiceId, opt => opt.MapFrom(s => s.ServiceId))
                .ForMember(d => d.ClaimId, opt => opt.MapFrom(s => s.ClaimId))
                .ReverseMap();
        }
    }
}