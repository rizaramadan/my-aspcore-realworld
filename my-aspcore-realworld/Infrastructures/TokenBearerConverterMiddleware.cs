using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace my_aspcore_realworld.Infrastructures
{
    public class TokenBearerConverterMiddleware
    {
        const string Authorization = nameof(Authorization);
        private readonly RequestDelegate _next;

        public TokenBearerConverterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Asp.net core is harcoding prefit "Bearer" for auth token. Since we may receive "Token" instead, we need to convert it 
        public async Task Invoke(HttpContext httpContext)
        {
            string authorization = httpContext.Request.Headers[Authorization];
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                httpContext.Request.Headers.Remove(Authorization);
                httpContext.Request.Headers.Add(Authorization, authorization.Replace("Token", "Bearer", StringComparison.OrdinalIgnoreCase));
            }
            await _next.Invoke(httpContext);
        }
    }
}   
