using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Infrastructures.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {  }
    }
}
