using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Security;

namespace Nocturne.Features.Users
{
    [ApiController]
    [Route("[controller]/api")]
    [Authorize(Policy = AuthorizeConstants.Policies.Administrator, Roles = AuthorizeConstants.Roles.Administrator)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mapper)
        {
            _mediator = mapper;
        }

        /// <summary>
        /// Get users paged list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Count of users on page</param>
        /// <returns></returns>
        [HttpGet("page/")]
        public async Task<IEnumerable<CoreUser>> GetUsers([FromQuery]int page, [FromQuery]int pageSize) =>
           await _mediator.Send(new GetUsers.Command(page, pageSize));

        [HttpGet("{userName}")]
        public async Task<CoreUser> GetUser([FromRoute] string userName) =>
           await _mediator.Send(new GetUser.Command(userName));

        [AllowAnonymous]
        [HttpPost("/user/{user}/online")]
        public async Task<bool> GetUserOnline([FromBody]User user) =>
           await _mediator.Send(new GetUserOnline.Command(user));


        [HttpPost("delete/")]
        public async Task<bool> DeleteUser([FromQuery]Guid id) =>
            await _mediator.Send(new DeleteUser.Command(id));

        [HttpPost("create")]
        public async Task<bool> CreateUser([FromBody]CoreUser user, string role) =>
            await _mediator.Send(new CreateUser.Command(user, role));
    }
}
