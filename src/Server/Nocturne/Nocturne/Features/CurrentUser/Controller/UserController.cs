using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Security;
using System.Security.Claims;

namespace Nocturne.Features.CurrentUser.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = $"{AuthorizeConstants.Policies.User}", Roles = $"{AuthorizeConstants.Roles.User}")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly UserManager<CoreUser> _userManager;

        private readonly SignInManager<EntityUser> _signInManager;

        public UserController(IMediator mediator, UserManager<CoreUser> userManager, SignInManager<EntityUser> signInManager)
        {
            _mediator = mediator;

            _userManager = userManager;

            _signInManager = signInManager;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<JwtAuthResult> SignIn([FromBody] CoreUser user)
        {
            return await _mediator.Send(new Command(user));
        }

        [HttpPost("create-one")]
        [AllowAnonymous]
        public async Task<JwtAuthResult> CreateAccount([FromBody] CoreUser user)
        {
            return await _mediator.Send(new CreateAccount.Command(user));
        }

        [AllowAnonymous]
        [HttpPost("external-sign-in")]
        public async Task<IActionResult> ExternalSignIn(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        [HttpGet("external-sign-in-callback")]
        public async Task<bool> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);

            return result.Succeeded;
        }

        [HttpGet("groups")]
        public async Task<IEnumerable<Group>> GetUserGroups()
        {
            return await _mediator.Send(new GetUserGroups.Command(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }
    }
}
