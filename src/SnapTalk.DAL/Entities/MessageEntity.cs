namespace SnapTalk.Domain.Entities;

public class MessageEntity
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public string? AttachmentFileName { get; set; }
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public Guid? ReplyToMessageId { get; set; }
    public MessageEntity? ReplyToMessage { get; set; }
    public ICollection<MessageEntity> Replies { get; set; } = new List<MessageEntity>();
    public UserEntity User { get; set; }
    public ChatEntity Chat { get; set; }
}