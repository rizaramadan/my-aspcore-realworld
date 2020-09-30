using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using my_aspcore_realworld.Entities;
using my_aspcore_realworld.Usecases;

namespace my_aspcore_realworld.Infrastructures
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<UserService> _l;
        private readonly IConfiguration _config;

        public UserService(
            ILogger<UserService> l,
            UserManager<AppUser> u,
            IConfiguration c,
            SignInManager<AppUser> m)
        {
            _l = l;
            _userManager = u;
            _config = c;
            _signInManager = m;
        }

        public async Task<bool> RegisterNew(AppUser user)
        {
            IdentityResult result = await _userManager.CreateAsync(user);
            return result.Succeeded;
        }

        public string GenerateTokenFrom(AppUser user)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);

                Claim[] claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("O")),
                    new Claim(JwtRegisteredClaimNames.NameId, $"{user.Id}"),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                _l.LogError($"fail to generate token from user {user.Id}, returning null", user, ex);
                return null;
            }
        }

        public async Task<AppUser> Login(AppUser user)
        {
            AppUser result = await _userManager.FindByEmailAsync(user.Email);
            if (result.Password.Equals(user.Password, StringComparison.OrdinalIgnoreCase))
            {
                await _signInManager.SignInAsync(result, isPersistent: false);
                _l.LogDebug("successfully signing in", result);
            }
            else
            {
                _l.LogDebug("fail to signing in", user);
            }
            return result.ClearSensitiveProperties();
        }

        public async Task<AppUser> GetUserById(long id) => await _userManager.FindByIdAsync($"{id}");
    }
}
