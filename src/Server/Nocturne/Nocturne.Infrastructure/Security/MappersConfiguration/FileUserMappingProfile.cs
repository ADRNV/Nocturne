using AutoMapper;
using Nocturne.Core.Models;

namespace Nocturne.Infrastructure.Security.MappersConfiguration
{
    public class FileUserMappingProfile : Profile
    {
        public FileUserMappingProfile()
        {
            CreateMap<FileUser, Infrastructure.Security.Entities.User>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.Email, opt => opt.MapFrom(fu => fu.Login))
                .ForMember(u => u.UserName, opt => opt.MapFrom(fu => fu.UserName))
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(fu => fu.Pasword))
                .ForMember(u => u.UserGroups, opt => opt.MapFrom(u => new Group[] {}))
                .ReverseMap()
                .ForMember(fu => fu.Role, opt => opt.Ignore());
        }
    }
}
