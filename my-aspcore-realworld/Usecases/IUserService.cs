using System.Threading.Tasks;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public interface IUserService
    {
        public Task<bool> RegisterNew(AppUser user);
        public string GenerateTokenFrom(AppUser user);
        public Task<AppUser> Login(AppUser user);
        public Task<AppUser> GetUserById(long id);
    }
}
