using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EFCorePostgres.Controllers.ResponseModels;
using EFCorePostgres.Data;
using EFCorePostgres.Models;
using EFCorePostgres.Models.Enums;
using EFCorePostgres.Services;
using EFCorePostgres.StartupConfigurations;
using System.Diagnostics;
using System.Security.Claims;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Http.HttpResults;
using EFCorePostgres.Controllers.APIResponses;

namespace EFCorePostgres.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FacebookController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly ILoginService _loginService;

        public FacebookController(
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

        // https://localhost:7209/api/facebook/login-facebook
        [HttpGet("login-facebook")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(FacebookCallBack)),
            };

            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        // MORE WORK PROMPT TO ENTER ADDRESS AND PASSWORD 
        [HttpGet("FacebookAuthentication")]
        [AllowAnonymous]
        public async Task<IResult> FacebookCallBack()
        {

            var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return Results.BadRequest("No authentication result");


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

            return await _loginService.ExternalLogin(FacebookDefaults.AuthenticationScheme, payload, HttpContext);

        }
    }
}
