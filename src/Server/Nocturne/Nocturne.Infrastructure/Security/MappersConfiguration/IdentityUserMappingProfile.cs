using AutoMapper;

namespace Nocturne.Infrastructure.Security.MappersConfiguration
{
    public class IdentityUserMappingProfile : Profile
    {
        public IdentityUserMappingProfile()
        {
            CreateMap<Core.Models.User, Infrastructure.Security.Entities.User>()
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(u => u.Pasword))
                .ForMember(u => u.Email, opt => opt.MapFrom(u => u.Login))
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.UserGroups, opt => opt.MapFrom(u => u.Groups))
                .ReverseMap();
        }
    }
}
