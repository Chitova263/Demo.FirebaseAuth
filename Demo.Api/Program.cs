using Consul;
using Demo.Api.Services;
using Demo.Shared;
using Demo.Shared.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var projectName = configuration["Firebase:ProjectName"];
    options.Authority = $"https://securetoken.google.com/{projectName}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://securetoken.google.com/{projectName}",
        ValidateAudience = true,
        ValidAudience = projectName,
        ValidateLifetime = true
    };
});

builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(configuration["Consul:Address"]);
}));

builder.Services.AddHostedService<ConsulRegisterService>();

builder.Services.Configure<ConsulConfiguration>(configuration.GetSection("Consul"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();