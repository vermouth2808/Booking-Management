using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Domain.Entities;
using Core.Infrastructure.Mappings;
using Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.Movie;
using StackExchange.Redis;
using Core.Infrastructure.Redis;
using Core.Shared.DTOs.Response.ShowTime;
using Core.Shared.DTOs.Response.Banner;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình JWT từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<BookMovieTicketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "bmt_";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(redisConnection);
});

// Thêm dịch vụ xác thực JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
        ClockSkew = TimeSpan.Zero 
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };

});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});



builder.Services.AddScoped<IMovieMapper<MovieRes>, MovieMapper<MovieRes>>();
builder.Services.AddScoped<IMovieService<MovieRes>, MovieService<MovieRes>>();
builder.Services.AddScoped<IMovieRepository<MovieRes>, MovieRepository<MovieRes>>();

builder.Services.AddScoped<IBannerMapper<BannerRes>, BannerMapper<BannerRes>>();
builder.Services.AddScoped<IBannerRepository<BannerRes>, BannerRepository<BannerRes>>();

builder.Services.AddScoped<IShowtimeMapper<ShowTimeRes>, ShowtimeMapper<ShowTimeRes>>();
builder.Services.AddScoped<IShowTimeService<ShowTimeRes>, ShowTimeService<ShowTimeRes>>();
builder.Services.AddScoped<IShowTimeRepository<ShowTimeRes>, ShowTimeRepository<ShowTimeRes>>();

builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();





builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme,Array.Empty<string>() }
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

builder.Services.AddAuthorization();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();