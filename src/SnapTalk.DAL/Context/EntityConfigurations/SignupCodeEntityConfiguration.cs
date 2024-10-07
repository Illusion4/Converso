using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnapTalk.Domain.Entities;

namespace SnapTalk.Domain.Context.EntityConfigurations;

public class SignupCodeEntityConfiguration : IEntityTypeConfiguration<SignupCodeEntity>
{
    public void Configure(EntityTypeBuilder<SignupCodeEntity> builder)
    {
        builder.Property(x => x.Email).ValueGeneratedNever();
        builder.HasKey(x => x.Email);
    }
}