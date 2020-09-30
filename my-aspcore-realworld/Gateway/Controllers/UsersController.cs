using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Gateway.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _l;
        private readonly IMediator _mediator;
        public static readonly Func<AppUser, JsonResult> JsonOf = (user) =>
        {
            return new JsonResult(new
            {
                user = new
                {
                    user.Email,
                    username = user.UserName,
                    user.Bio,
                    user.Image,
                    user.Token
                }
            });
        };

        public UsersController(ILogger<UsersController> l, IMediator m)
        {
            _l = l;
            _mediator = m;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _l.LogInformation($"{nameof(Get)} is called");
            return Ok("Oke");
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(UsersRegister user)
        {
            AppUser result = await _mediator.Send(user);
            return JsonOf(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UsersLogin user)
        {
            AppUser result = await _mediator.Send(user);
            return JsonOf(result);
        }
    }
}
