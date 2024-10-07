using Microsoft.EntityFrameworkCore;
using SnapTalk.BLL.Interfaces;
using SnapTalk.BLL.Services;
using SnapTalk.Domain.Context;
using SnapTalk.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddJwtGeneratorOptions(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SnapTalkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(SnapTalkContext).Assembly.FullName));
});

builder.Services.AddSingleton(new EmailConfig("gaston.gleason75@ethereal.email", "7zEqZk92Rf8BV4k4HA",
    "smtp.ethereal.email", 587));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IOtpService, OtpService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDatabaseCoreContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
