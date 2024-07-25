using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context.EntityConfigurations;

public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId);
        
        builder.HasMany(x => x.UserChats)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId);
    }
}