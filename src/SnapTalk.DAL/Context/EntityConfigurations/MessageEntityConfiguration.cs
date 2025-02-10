using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context.EntityConfigurations;

public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.HasMany(x => x.Replies)
            .WithOne(x => x.ReplyToMessage)
            .HasForeignKey(x => x.ReplyToMessageId);
    }
}