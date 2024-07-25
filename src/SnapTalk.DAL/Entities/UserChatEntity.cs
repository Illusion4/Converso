using SnapTalk.Domain.Entities.Enums;

namespace SnapTalk.Domain.Entities;

public class UserChatEntity
{
    public Guid UserId { get; set; }
    
    public Guid ChatId { get; set; }
    
    public UserChatRole Role { get; set; }
    public UserEntity User { get; set; }
    
    public ChatEntity Chat { get; set; }
}