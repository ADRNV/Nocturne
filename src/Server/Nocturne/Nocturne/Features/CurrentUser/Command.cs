using MediatR;
using Nocturne.Infrastructure.Security;

namespace Nocturne.Features.CurrentUser
{
    public record Command(CoreUser User) : IRequest<JwtAuthResult>;
}
