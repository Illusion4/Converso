using SnapTalk.Domain.Entities.Enums;

namespace SnapTalk.Common.DTO;

public record ChatResponse(
    Guid ChatId,
    string Title,
    string? Description,
    ChatType ChatType,
    int UsersCount,
    bool IsMember,
    string LastMessage,
    DateTime? LastMessageDate,
    string LastMessageSender,
    string ChatImageUri
    );