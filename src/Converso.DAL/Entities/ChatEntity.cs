using SnapTalk.Domain.Entities.Enums;

namespace SnapTalk.Domain.Entities;

public class ChatEntity
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required string ImageFileName { get; set; }

    public required ChatType ChatType { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    
    public ICollection<UserChatEntity> UserChats { get; set; } = new List<UserChatEntity>();
}