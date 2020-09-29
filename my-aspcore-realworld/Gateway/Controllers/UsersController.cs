using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _l;
        private readonly IMediator _mediator;

        public UsersController(ILogger<UsersController> l, IMediator m)
        {
            _l = l;
            _mediator = m;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            _l.LogInformation($"{nameof(Get)} is called");
            return Ok("Oke");
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync(UsersRegister user)
        {
            AppUser result = await _mediator.Send(user);
            return new JsonResult(new
            {
                user = new
                {
                    result.Email,
                    username = result.UserName,
                    result.Bio,
                    image = string.Empty,
                    result.Token
                }
            });
        }
    }
}
