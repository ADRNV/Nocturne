using AutoMapper;
using Nocturne.Infrastructure.Messaging.Models;

namespace Nocturne.Infrastructure.Security.MappersConfiguration
{
    public class MessagesMappingProfile : Profile
    {
        public MessagesMappingProfile()
        {
            CreateMap<CoreMessage, Message>()
                .ForMember(m => m.User, opt => opt.Ignore())
                .ForMember(m => m.Content, opt => opt.MapFrom(m => m.Content))
                .ReverseMap()
                .ForMember(m => m.From, opt => opt.MapFrom(m => m.User.UserName));
        }
    }
}
