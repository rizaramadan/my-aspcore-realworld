﻿using Microsoft.AspNetCore.Identity;

namespace my_aspcore_realworld.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public string Password { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }

        public AppUser NullizeProperties()
        {
            Bio ??= string.Empty;
            Token ??= string.Empty;
            return this;
        }
    }
}
