using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Nocturne.Core.Managers;
using Nocturne.Core.Repositories;
using Nocturne.Features.Messaging.Clients;
using Nocturne.Features.Messaging.Validation;
using Nocturne.Infrastructure.Messaging.Models;
using Nocturne.Infrastructure.Messaging.Storage;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Features.Messaging.Hubs
{
    public class SendMessage
    {
        public record Command(HubBase<IChatClient> HubContext, CoreMessage Message, string To) : IRequest<bool>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Message).SetValidator(new MessageValidator());
                RuleFor(c => c.To).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IConnectionsManager _connectionsManager;

            private readonly UserManager<User> _userManager;

            private readonly IMessagesRepository<Message> _messagesStore;

            private readonly IMapper _mapper;

            public Handler(IConnectionsManager connectionsManager, UserManager<User> userManager, IMessagesRepository<Message> messagesStore, IMapper mapper)
            {
                _connectionsManager = connectionsManager;

                _userManager = userManager;

                _messagesStore = messagesStore;

                _mapper = mapper;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var identityUser = await _userManager.FindByNameAsync(request.To);

                if (identityUser is not null)
                {
                    var user = _mapper.Map<User>(identityUser);

                    var userConnections = await _connectionsManager
                        .GetUserConnections(_mapper.Map<Core.Models.User>(user));

                    request.Message.From = user.UserName;

                    await _messagesStore.CreateMessage(identityUser.Id,
                        _mapper.Map<CoreMessage, Message>(request.Message));

                    await request.HubContext.Clients.Clients(userConnections)
                        .SendMessage(request.Message);
                }

                return identityUser is null;
            }
        }
    }
}
