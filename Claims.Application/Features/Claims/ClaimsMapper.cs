using AutoMapper;
using Claims.Application.Features.Claims.Commands;
using Claims.Domain.Entities;

namespace Claims.Application.Features.Claims
{
    public class ClaimsMapper: Profile
    {
        public ClaimsMapper()
        {
            CreateMap<Claim, NewClaim.Command>();
            CreateMap<NewClaim.Command, Claim>()
                .ForMember(claim => claim.Id, opt => opt.Ignore());
            
        }
    }
}