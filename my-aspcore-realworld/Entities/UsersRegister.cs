using MediatR;

namespace my_aspcore_realworld.Entities
{
    public class UsersRegister : IRequest<AppUser>
    {
        public AppUser User { get; set; }
    }
}
