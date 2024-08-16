namespace EFCorePostgres.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;
    using EFCorePostgres.Models;
    using EFCorePostgres.Services;
    using Microsoft.AspNetCore.Identity.Data;
    using EFCorePostgres.Controllers.RequestModels;
    using EFCorePostgres.Data;
    using EFCorePostgres.Models.Enums;
    using EFCorePostgres.Models.DTOs;
    using NuGet.Packaging;
    using System.Security.Claims;
    using Microsoft.EntityFrameworkCore;
    using NuGet.Common;
    using EFCorePostgres.Controllers.ResponseModels;
    using Microsoft.AspNetCore.Authorization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Diagnostics;
    using EFCorePostgres.Attributes;
    using Microsoft.Extensions.Options;
    using EFCorePostgres.StartupConfigurations;
    using Microsoft.AspNetCore.Authentication.Google;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using System.Configuration.Provider;
    using Microsoft.DotNet.Scaffolding.Shared;
    using Mono.TextTemplating;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Client> _userManager;
        private readonly AppDbContext _context;
        private readonly ILoginService _loginService;
        public AuthenticationController(
            UserManager<Client> userManager,
            AppDbContext context,
            ILoginService loginService)

        {
            _userManager = userManager;
            _context = context;
            _loginService = loginService;
        }

        [HttpPost("login")]
        //[SkipJwtTokenMiddleware]
        public async Task<IResult> Login(LoginCustomRequest model)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            return await _loginService.Login(model, HttpContext);
           
        }

        

        [HttpPost("logout")]
        public  IResult Revoke()
        {
            return _loginService.Logout(HttpContext);
        }


        [HttpPost("register")]
        [SkipJwtTokenMiddleware]
        public async Task<IActionResult> Register(RegisterCustomRequest model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Client()
            {
                Email = model.Email,
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, Role.ROLE_USER.ToString());

                if (result.Succeeded)
                    return Ok(new { result = "User created successfully", user = new ClientDTO(user) });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("registerAdmin")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> RegisterAdmin(RegisterCustomRequest model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Client()
            {
                Email = model.Email,
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user,
                                                new[] { Role.ROLE_USER.ToString(), Role.ROLE_ADMIN.ToString() });

                if (result.Succeeded)
                    return Ok(new { result = "User created successfully", user = new ClientDTO(user) });
            }

            return BadRequest(result.Errors);

        }
    }
}



