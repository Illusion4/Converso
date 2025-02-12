using System.Text;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SnapTalk.BLL.Interfaces;
using SnapTalk.BLL.Services;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

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
        

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
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
        services.AddScoped<IChatsService, ChatsService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IAvatarService, AvatarService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<IOtpService, OtpService>();
        
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
    
    public static IServiceCollection AddAzureBlobServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var blobUrl = configuration["BlobConnectionString"];
        var blobContainerName = configuration["BlobContainerName"];
        var blobAccessUrl= configuration["BlobAccessUrl"];

        var settings = new BlobStorageSettings(blobUrl!, blobContainerName!, blobAccessUrl!);
        var blobContainerClient = new BlobContainerClient(settings.BlobConnectionString, settings.BlobContainerName);

        services.AddSingleton(_ => settings);

        services.AddSingleton(_ => blobContainerClient);

        services.AddScoped<IBlobService, BlobService>();

        return services;
    }
    
    public static IServiceCollection ConfigureCustomValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => 
                        new Error("VALIDATION_ERROR", 
                            e.ErrorMessage,
                            new Dictionary<string, object> { { "field", x.Key } })))
                    .ToList();

                var response = Response<object>.Error(errors);

                return new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            };
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer", },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }
}