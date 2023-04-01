using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Security;


namespace Nocturne.Features.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-in")]
        public async Task<JwtAuthResult> SignIn([FromBody] User user)
        {
            return await _mediator.Send(new SignIn.Command(user));
        }

        [HttpPost("create-one")]
        public async Task<JwtAuthResult> CreateAccount([FromBody] User user)
        {
            return await _mediator.Send(new CreateAccount.Command(user));
        }

    }
}
