namespace SnapTalk.Common.DTO;

public record MessageResponse
(
    Guid UserId,
    Guid ChatId,
    Guid MessageId,
    string DisplayedName,
    string Message,
    string DisplayedNameColor,
    string? AttachmentUrl,
    string? UserAvatarUrl,
    
    string? ReplyToMessage,
    string? ReplyToUser,
    string? ReplyToUserColor,
    
    DateTime CreatedAt,
    DateTime? UpdatedAt
);