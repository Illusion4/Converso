using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context.EntityConfigurations;

public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.Property(e => e.ExpiresAt).IsRequired();
        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}