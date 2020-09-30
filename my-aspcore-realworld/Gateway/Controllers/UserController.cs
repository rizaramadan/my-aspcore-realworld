using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_aspcore_realworld.Entities;
using my_aspcore_realworld.Usecases;

namespace my_aspcore_realworld.Gateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService u) => _userService = u;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string userId = User.Claims
                .Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value != "my-conduit")
                .FirstOrDefault()?.Value;
            if (long.TryParse(userId, out long id))
            {
                AppUser user = await _userService.GetUserById(id);
                user.Token = _userService.GenerateTokenFrom(user);
                return UsersController.JsonOf(user);
            }
            return NoContent();
        }
    }
}
