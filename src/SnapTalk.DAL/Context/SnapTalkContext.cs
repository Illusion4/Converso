using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context;

public class SnapTalkContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    
    public SnapTalkContext(DbContextOptions<SnapTalkContext> options) : base(options)
    { }
    
    public DbSet<ChatEntity> Chats { get; set; }
    
    public DbSet<UserChatEntity> UserChats { get; set; }
    
    public DbSet<MessageEntity> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}