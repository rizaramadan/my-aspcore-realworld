using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using my_aspcore_realworld.Infrastructures.Persistence;
using my_aspcore_realworld.Usecases;

namespace my_aspcore_realworld.Infrastructures
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddMyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreConnection")));

            services.AddScoped<IUserService, UserService>();
            services.AddMediatR(typeof(Startup));
        }
    }
}
