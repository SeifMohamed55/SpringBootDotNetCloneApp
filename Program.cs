using Microsoft.EntityFrameworkCore;
using EFCorePostgres.Data;
using Microsoft.AspNetCore.Identity;
using EFCorePostgres.Models;
using EFCorePostgres.Services;
using EFCorePostgres.Middleware;
using EFCorePostgres.StartupConfigurations;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<AppDbContext>
                            (x => x.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlConstr")));

builder.Services.AddIdentity<Client, Authority>()
    .AddRoles<Authority>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICachingService, CachingService>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

await builder.Services.AddJwtGoogleAuthentication(builder.Configuration);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRsaCertficate, SigningIssuerCertficate>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Use a cookie to temporarily store information about a user logging in with a third party login provider

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseJwtTokenValidationMiddleWare();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
