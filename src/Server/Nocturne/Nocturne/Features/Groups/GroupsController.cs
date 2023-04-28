using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Security;

namespace Nocturne.Features.Groups
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = $"{AuthorizeConstants.Policies.User}", Roles = $"{AuthorizeConstants.Roles.User}, {AuthorizeConstants.Roles.Administrator}")]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("groups/add")]
        public async Task<bool> AddUserToGroup(string userName, Group group)
        {
            return await _mediator.Send(new Command(userName, group));
        }

        [HttpPost("groups/remove")]
        public async Task<bool> RemoveUserFromGroup(string userName, Group group)
        {
            return await _mediator.Send(new Command(userName, group));
        }
    }
}
