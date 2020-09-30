using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public interface IUserService
    {
        public Task<bool> RegisterNewUser(AppUser user);
        public string GenerateTokenFrom(AppUser user);
    }
}
