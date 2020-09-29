using System;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using my_aspcore_realworld.Entities;
using my_aspcore_realworld.Infrastructures.Persistence;

namespace my_aspcore_realworld
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.IgnoreNullValues = true;
                x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddIdentity<AppUser, IdentityRole<long>>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddMediatR(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseCors(x => x
               .SetIsOriginAllowed(_ => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
