using AutoMapper;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Infrastructure.Security.MappersConfiguration
{
    public class UserGroupMappingProfile : Profile
    {
        public UserGroupMappingProfile()
        {
            CreateMap<Core.Models.Group, UserGroup>()
                .ReverseMap();
        }
    }
}
