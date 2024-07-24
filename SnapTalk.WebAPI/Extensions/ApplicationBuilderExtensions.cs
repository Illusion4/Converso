using Microsoft.EntityFrameworkCore;
using SnapTalk.Domain.Context;

namespace SnapTalk.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseDatabaseCoreContext(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        using var context = scope?.ServiceProvider.GetRequiredService<SnapTalkContext>();
        context?.Database.Migrate();
    }
}