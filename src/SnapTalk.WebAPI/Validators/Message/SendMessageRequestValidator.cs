using FluentValidation;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Validators.Message;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message cannot be empty.")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.");

        RuleFor(x => x.ChatId)
            .NotEmpty().WithMessage("ChatId is required.");

        RuleFor(x => x.ReplyToMessageId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("ReplyToMessageId must be a valid GUID or null.");

        RuleFor(x => x.FileAttachment)
            .Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024)) // Max 5MB
            .WithMessage("FileAttachment must be a valid file and not exceed 5MB.");
    }
}