using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Infrastructure.Security;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Features.CurrentUser.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User", Roles = "User, Administrator")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public UserController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
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

        [HttpGet("check")]
        public string Check() => "work";

    }
}
