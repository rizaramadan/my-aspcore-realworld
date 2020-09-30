using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
                r.User.Token = _userService.GenerateTokenFrom(r.User);
                return r.User;
            }
            return null;
        }
    }
}
