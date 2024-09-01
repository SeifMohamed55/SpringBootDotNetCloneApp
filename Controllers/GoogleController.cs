using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpringBootCloneApp.Controllers.ResponseModels;
using SpringBootCloneApp.Data;
using SpringBootCloneApp.Models;
using SpringBootCloneApp.Models.Enums;
using SpringBootCloneApp.Services;
using SpringBootCloneApp.StartupConfigurations;
using System.Diagnostics;
using System.Security.Claims;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authentication.Facebook;
using SpringBootCloneApp.Controllers.APIResponses;

namespace SpringBootCloneApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly ILoginService _loginService;

        public GoogleController(
            IJwtTokenService tokenService,
            ILoginService loginService)

        {
            _tokenService = tokenService;
            _loginService = loginService;
        }


        [HttpGet("AccessDeniedPathInfo")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return BadRequest("Authentication Failed");
        }

        // https://localhost:7209/api/google/login-google
        [HttpGet("login-google")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallBack)),
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // MORE WORK PROMPT TO ENTER ADDRESS AND PASSWORD 
        [HttpGet("GoogleAuthentication")]
        [AllowAnonymous]
        public async Task<IResult> GoogleCallBack()
        {

            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return Results.BadRequest("No authentication result");

            var x= User.Claims;
            var claimsPrincipal = authenticateResult.Principal;

            if (claimsPrincipal == null)
                return Results.BadRequest("No principal available");

            var payload = new AuthenticatedPayload
            {
                Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                Email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "",
                GivenName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value ?? "",
                FamilyName = claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value ?? ""
            };

            if (payload == null)
                return Results.BadRequest();

            return await _loginService.ExternalLogin(GoogleDefaults.AuthenticationScheme, payload, HttpContext);

        }
    }
}
