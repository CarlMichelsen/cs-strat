using System.Text;
using Api.Extension;
using Api.Hubs;
using Api.Middleware;
using BusinessLogic.Database;
using BusinessLogic.Factory;
using BusinessLogic.LobbyManagement;
using BusinessLogic.Handler;
using BusinessLogic.Repository;
using BusinessLogic.Service;
using Domain.Configuration;
using Interface.Handler;
using Interface.Factory;
using Interface.Repository;
using Interface.LobbyManagement;
using Interface.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseInMemoryDatabase("ApplicationDatabase"));

// Handler
builder.Services
    .AddTransient<ILobbyHandler, LobbyHandler>()
    .AddTransient<IUserHandler, UserHandler>();

// Service
builder.Services
    .AddTransient<IHumanReadableIdentifierService, HumanReadableIdentifierService>()
    .AddTransient<IJwtService, JwtService>()
    .AddTransient<ILobbyService, LobbyService>()
    .AddTransient<ILobbyAuthService, LobbyAuthService>()
    .AddSignalR();

// Factories
builder.Services
    .AddScoped<ILobbyAuthServiceFactory, LobbyAuthServiceFactory>();

// Lobby Management
builder.Services
    .AddTransient<ILobbyStateMachine, LobbyStateMachine>()
    .AddTransient<ILobbyManager, LobbyManager>();

// Repository
builder.Services
    .AddTransient<ILobbyAccessRepository, LobbyAccessRepository>();

// Middleware
builder.Services
    .AddTransient<CookieMiddleware>()
    .AddTransient<SignalRLobbyMiddleware>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGenWithXmlDocumentation(options =>
    options.CustomSchemaIds(SwaggerExtension.GetSchemaIdRecursively));

builder.Services.AddEndpointsApiExplorer();

// Configuration
builder.Configuration.AddJsonFile("secrets.json", true);

builder.Services
    .AddOptions<SwaggerOptions>()
    .Bind(builder.Configuration.GetSection(SwaggerOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<CorsOptions>()
    .Bind(builder.Configuration.GetSection(CorsOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Authorization
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName);
        var keyString = jwtOptions[nameof(JwtOptions.Key)];
        var key = Encoding.UTF8.GetBytes(keyString!);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],
            ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(ApplicationConstants.FrontendCorsPolicy, corsBuilder =>
    {
        var corsOptions = builder.Configuration.GetSection(CorsOptions.SectionName);
        var commaSeparatedOrigins = corsOptions[nameof(CorsOptions.WithOrigins)]
            ?? throw new NullReferenceException("No origins in configuration");

        var origins = commaSeparatedOrigins.Split(',')
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Select(origin => origin.Trim())
            .ToArray();

        corsBuilder.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 3;
        options.QueueLimit = 0;
    });
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
var swaggerOptions = app.Services.GetService<IOptions<SwaggerOptions>>();

if (app.Environment.IsDevelopment() || swaggerOptions!.Value.Enabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseCors(ApplicationConstants.FrontendCorsPolicy);

app.UseMiddleware<CookieMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<SignalRLobbyMiddleware>();

app.MapControllers();

app.MapHub<LobbyHub>(ApplicationConstants.LobbySignalREndpoint);

app.Run();
