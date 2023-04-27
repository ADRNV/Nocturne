using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Features.Users
{
    [ApiController]
    [Route("[controller]/api")]
    [Authorize(Policy = "Administrator", Roles = "Administrator")]
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

        [HttpPost("delete/")]
        public async Task<bool> DeleteUser([FromQuery]Guid id) =>
            await _mediator.Send(new DeleteUser.Command(id));

        [HttpPost("create")]
        public async Task<bool> CreateUser([FromBody]CoreUser user, string role) =>
            await _mediator.Send(new CreateUser.Command(user, role));
    }
}
