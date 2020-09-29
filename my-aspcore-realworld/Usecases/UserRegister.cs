using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public class UserRegisterHandler : IRequestHandler<UsersRegister, AppUser>
    {
        private readonly ILogger<UserRegisterHandler> _l;
        private readonly UserManager<AppUser> _userManager;

        public UserRegisterHandler(ILogger<UserRegisterHandler> l, UserManager<AppUser> um)
        {
            _l = l;
            _userManager = um;
        }

        public async Task<AppUser> Handle(UsersRegister r, CancellationToken ct)
        {
            IdentityResult result = await _userManager.CreateAsync(r.User);
            if (result.Succeeded)
                return r.User.NullizeProperties();
            return null;
        }
    }
}
