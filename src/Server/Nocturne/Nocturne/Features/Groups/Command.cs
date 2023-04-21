using MediatR;

namespace Nocturne.Features.Groups
{
    public record Command(string UserName, CoreGroup Group) : IRequest<bool>;
}
