using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Common;
using SpringBootCloneApp.Attributes;
using SpringBootCloneApp.Controllers.RequestModels;
using SpringBootCloneApp.Controllers.ResponseModels;
using SpringBootCloneApp.Data;
using SpringBootCloneApp.Models;
using SpringBootCloneApp.Services;
using SpringBootCloneApp.StartupConfigurations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace SpringBootCloneApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _tokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly ICachingService _cachingService;
        public TokenController
            (AppDbContext context,
            IJwtTokenService tokenService,
            IOptions<JwtOptions> options,
            ICachingService cachingService)
        {
            _context = context;
            _tokenService = tokenService;
            _jwtOptions = options.Value;
            _cachingService = cachingService;
        }

        // needs WORK
        [HttpPost("refresh")]
        [SkipJwtTokenMiddleware]
        public async Task<IActionResult> Refresh()
        {

            string? oldAccessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (oldAccessToken == null)
                return BadRequest("No Token Provided");

            var extractedId =   _tokenService.ExtractIdFromExpiredToken(oldAccessToken);

            if (extractedId is null)
                return BadRequest("Provided token is invalid");

            var user = await _context.Clients
                .Include(x => x.Authorities)
                .FirstOrDefaultAsync(x => x.Id == extractedId);

            if (user == null)
                return BadRequest("Provided token is invalid");

            var oldRefreshToken = _cachingService.GetRefreshToken(oldAccessToken);

            var cookieRefreshToken = HttpContext.Request.Cookies["refreshToken"];

            if (oldRefreshToken is null || cookieRefreshToken is null || oldRefreshToken != cookieRefreshToken)
                return Unauthorized("Invalid client request You have to login!");


            string newAccessToken =  _tokenService.GenerateSymmetricJwtToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _cachingService.AddRefreshTokenCachedData(newAccessToken, newRefreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Value, cookieOptions);

            return Ok(new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                ValidTo = DateTime.UtcNow.AddMinutes(double.Parse(_jwtOptions.AccessTokenValidity)).ToString("f")
            });
        }
        
    }
}
