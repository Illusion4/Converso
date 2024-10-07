using Microsoft.AspNetCore.Identity;

namespace SnapTalk.Domain.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Bio { get; set; }
    
    public ICollection<UserChatEntity> UserChats { get; set; } = new List<UserChatEntity>();
    
    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    
    public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
}