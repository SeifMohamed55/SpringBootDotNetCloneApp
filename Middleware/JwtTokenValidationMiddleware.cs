using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EFCorePostgres.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EFCorePostgres.Middleware
{
    // kinda uselsee now for me
    public class JwtTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtTokenValidationMiddleware(RequestDelegate next, IOptions<TokenValidationParameters> tokenValidationParameters)
        {
            _next = next;
            _tokenValidationParameters = tokenValidationParameters.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var skip = endpoint.Metadata.GetMetadata<SkipJwtTokenMiddleware>(); // my attribute exists
                if (skip == null) // if skip exist skip the filter
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (token != null)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        try
                        {
                            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                            // Check if the token has expired
                            var jwtToken = validatedToken as JwtSecurityToken;
                            if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                await context.Response.WriteAsync("Token expired. Please refresh the token.");
                                return;
                            }
                        }
                        catch (SecurityTokenException)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Invalid token. Please refresh the token.");
                            return;
                        }
                    }
                }
            }
            await _next(context);

        }
    }
    // Extension method to add the middleware to the pipeline
    public static class JwtTokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenValidationMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenValidationMiddleware>();
        }
    }


}
