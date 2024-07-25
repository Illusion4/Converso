using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context.EntityConfigurations;

public class UserChatEntityConfiguration : IEntityTypeConfiguration<UserChatEntity>
{
    public void Configure(EntityTypeBuilder<UserChatEntity> builder)
    {
        builder.HasKey(x => new {x.UserId, x.ChatId});
    }
}