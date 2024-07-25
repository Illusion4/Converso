namespace SnapTalk.Domain.Entities;

public class ChatEntity
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; } = DateTime.Now;
    
    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    
    public ICollection<UserChatEntity> UserChats { get; set; } = new List<UserChatEntity>();
}