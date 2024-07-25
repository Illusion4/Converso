namespace SnapTalk.Domain.Entities;

public class MessageEntity
{
    public Guid Id { get; set; }
    
    public required string Content { get; set; }
    
    public DateTime CreatedAt { get; } = DateTime.Now;
    
    public Guid UserId { get; set; }
    
    public Guid ChatId { get; set; }
    
    public UserEntity User { get; set; }
    
    public ChatEntity Chat { get; set; }
}