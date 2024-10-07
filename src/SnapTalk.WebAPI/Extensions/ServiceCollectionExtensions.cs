using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SnapTalk.BLL.Interfaces;
using SnapTalk.BLL.Services;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;
using SnapTalk.WebAPI.Services;

namespace SnapTalk.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<SnapTalkContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
                };
            });

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        services.AddScoped<ICurrentContextProvider, CurrentContextProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        return services;
    }
    
    public static IServiceCollection AddJwtGeneratorOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtIssuer = configuration["Jwt:Issuer"]!;
        var jwtAudience = configuration["Jwt:Audience"]!;
        var jwtSecretKey = configuration["Jwt:SecretKey"]!;
        var jwtExpiration = configuration.GetValue<int>("Jwt:TokenExpiryInMinutes");
        var refreshTokenExpiration = configuration.GetValue<int>("Jwt:RefreshTokenExpiryInDays");
        
        var jwtOptions = new JwtOptions()
        {
            JwtIssuer = jwtIssuer,
            JwtAudience = jwtAudience,
            JwtSecretKey = jwtSecretKey,
            JwtExpiryInMinutes = jwtExpiration,
            RefreshTokenExpiryInDays = refreshTokenExpiration
        };
        services.AddSingleton<IJwtOptions>(jwtOptions);
        
        return services;
    }
}