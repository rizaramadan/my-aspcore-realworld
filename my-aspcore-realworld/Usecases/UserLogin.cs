using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public class UserLoginHandler : IRequestHandler<UsersLogin, AppUser>
    {
        private readonly ILogger<UserLoginHandler> _l;
        private readonly IUserService _userService;

        public UserLoginHandler(
            ILogger<UserLoginHandler> l,
            IUserService u)
        {
            _l = l;
            _userService = u;
        }

        public async Task<AppUser> Handle(UsersLogin r, CancellationToken ct)
        {
            AppUser loggedInUser = await _userService.Login(r.User);
            if (loggedInUser is null)
            {
                _l.LogDebug("user regsitration fail", r.User);
            }
            else
            {
                _l.LogDebug("user successfully registered", r.User);
                loggedInUser.Token = _userService.GenerateTokenFrom(loggedInUser);
            }
            return loggedInUser;
        }
    }
}
