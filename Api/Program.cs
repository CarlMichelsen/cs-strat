using System.Text;
using Api.Extension;
using Api.Hubs;
using Api.Middleware;
using Businesslogic.Database;
using Businesslogic.Handler;
using Businesslogic.Repository;
using Businesslogic.Service;
using Domain.Configuration;
using Interface.Handler;
using Interface.Repository;
using Interface.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    .AddSignalR();

// Repository
builder.Services
    .AddTransient<ILobbyRepository, LobbyRepository>();

// Middleware
builder.Services
    .AddTransient<CookieMiddleware>();

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
        var issuer = jwtOptions[nameof(JwtOptions.Issuer)];
        var audience = jwtOptions[nameof(JwtOptions.Audience)];

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
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
    options.AddPolicy("FrontendAllowed", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services
    .AddAuthorization();

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

app.UseCors("FrontendAllowed");

app.UseMiddleware<CookieMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<LobbyHub>("/LobbyHub");

app.Run();
