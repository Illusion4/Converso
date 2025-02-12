using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SnapTalk.Common.DTO;

public record SendMessageRequest(
    string Message,
    Guid ChatId,
    Guid? ReplyToMessageId,
    IFormFile? FileAttachment
    );