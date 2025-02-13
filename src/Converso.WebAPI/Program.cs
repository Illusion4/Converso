using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SnapTalk.BLL.Hubs;
using SnapTalk.BLL.Interfaces;
using SnapTalk.BLL.Services;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.WebAPI.Extensions;
using SnapTalk.WebAPI.Validators.User;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddJwtGeneratorOptions(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddDbContext<SnapTalkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAzureBlobServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.ConfigureCustomValidationResponse();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();
builder.Services.AddSignalR();
builder.Services.AddCors();


var app = builder.Build();

app.UseDatabaseCoreContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(opt => opt
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto,
});

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
