using Consul;
using Demo.IdentityService.Config;
using Demo.IdentityService.Services.AuthService;
using Demo.IdentityService.Services.EmailService;
using Demo.Shared;
using Demo.Shared.Consul;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    // Let the clients of the API know all supported versions.
    options.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDiscoveryClient();

var configuration = builder.Configuration;

builder
    .Services
    .Configure<FirebaseConfiguration>(configuration.GetSection("Firebase"));

builder
    .Services
    .AddHttpClient<IAuthService,AuthService>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri(configuration["Firebase:BaseUrl"]);
});

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile($"{Directory.GetCurrentDirectory()}/firebase.json")
});

builder.Services.AddTransient<IEmailService, EmailService>();

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

//app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();



