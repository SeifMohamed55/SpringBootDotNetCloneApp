using Microsoft.EntityFrameworkCore;
using SpringBootCloneApp.Data;
using Microsoft.AspNetCore.Identity;
using SpringBootCloneApp.Models;
using SpringBootCloneApp.Services;
using SpringBootCloneApp.Middleware;
using SpringBootCloneApp.StartupConfigurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                      policy.WithOrigins("https://webhook.site")
                            .AllowAnyMethod() 
                            .AllowAnyHeader();
                      });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<AppDbContext>
                            (x => x.UseNpgsql(builder.Configuration.GetConnectionString("OFFLINE_POSTGRESQL")));


builder.Services.AddIdentity<Client, Authority>()
    .AddRoles<Authority>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICachingService, CachingService>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddJwtGoogleAuthentication(builder.Configuration);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRsaCertficate, SigningIssuerCertficate>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmailingService, EmailingService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

// app.UseJwtTokenValidationMiddleWare();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



/*var environmentVariables = Environment.GetEnvironmentVariables().Cast<DictionaryEntry>().OrderBy(x => (string)x.Key, StringComparer.OrdinalIgnoreCase);

foreach (DictionaryEntry entry in environmentVariables)
{
    Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
}*/