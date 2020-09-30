using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public class UserRegisterHandler : IRequestHandler<UsersRegister, AppUser>
    {
        private readonly ILogger<UserRegisterHandler> _l;
        private readonly IUserService _userService;

        public UserRegisterHandler(
            ILogger<UserRegisterHandler> l,
            IUserService u)
        {
            _l = l;
            _userService = u;
        }

        public async Task<AppUser> Handle(UsersRegister r, CancellationToken ct)
        {
            bool isSuccess = await _userService.RegisterNew(r.User);
            if (isSuccess)
            {
                _l.LogDebug("user successfully registered", r.User);
                r.User.Token = _userService.GenerateTokenFrom(r.User);
                await _userService.Login(r.User);
                return r.User;
            }
            _l.LogDebug("user regsitration fail", r.User);
            return null;
        }
    }
}
