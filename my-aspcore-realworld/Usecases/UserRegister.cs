using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using my_aspcore_realworld.Entities;

namespace my_aspcore_realworld.Usecases
{
    public class UserRegisterHandler : IRequestHandler<UsersRegister, AppUser>
    {
        private readonly ILogger<UserRegisterHandler> _l;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public UserRegisterHandler(
            ILogger<UserRegisterHandler> l,
            UserManager<AppUser> um,
            IConfiguration c)
        {
            _l = l;
            _userManager = um;
            _config = c;
        }

        public async Task<AppUser> Handle(UsersRegister r, CancellationToken ct)
        {
            IdentityResult result = await _userManager.CreateAsync(r.User);
            if (result.Succeeded)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);

                Claim[] claims = new [] {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("O")),
                    new Claim(JwtRegisteredClaimNames.NameId, $"{r.User.Id}"),
                    new Claim(JwtRegisteredClaimNames.UniqueName, r.User.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, r.User.Email)
                };

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.NameId, $"{r.User.Id}"),
                        new Claim(JwtRegisteredClaimNames.UniqueName, r.User.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, r.User.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                r.User.Token = tokenHandler.WriteToken(token);
                return r.User.NullizeProperties();
            }
            return null;
        }
    }
}
