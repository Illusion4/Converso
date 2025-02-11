using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SnapTalk.BLL.Helpers;
using SnapTalk.BLL.Hubs;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;

namespace SnapTalk.BLL.Services;

public class MessageService(SnapTalkContext context,
    IHubContext<ChatHub, IHubClient> hubContext,
    ICurrentContextProvider httpContextProvider,
    IBlobService blobService, BlobStorageSettings blobStorageSettings) : IMessageService
{
    public async Task<Response<MessageResponse>> SendMessage(SendMessageRequest request)
    {
        var isUserMemberOfChat = await IsUserMemberOfChat(request.ChatId);

        if (!isUserMemberOfChat)
        {
            return ResponseErrors.NotMemberOfChat;
        }
        
        var user = await context.Users.FirstOrDefaultAsync( u => u.Id == httpContextProvider.CurrentUserId);

        if (user is null)
        {
            return ResponseErrors.UserNotFound;
        }
        
        var attachmentFileName = await UploadAttachmentAsync(request);
        
        var message = new MessageEntity
        {
            UserId = httpContextProvider.CurrentUserId,
            ChatId = request.ChatId,
            Content = request.Message,
            AttachmentFileName = attachmentFileName,
            ReplyToMessageId = request.ReplyToMessageId
        };

        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync();
        
        var replyToMessage = await context.Messages
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == request.ReplyToMessageId);
        
        var attachmentUrl = attachmentFileName is not null
            ? $"{blobStorageSettings.BlobAccessUrl}/{attachmentFileName}"
            : null;

        var profilePictureUrl = $"{blobStorageSettings.BlobAccessUrl}/{user.ProfilePictureFileName}";

        
        var response = new MessageResponse(message.UserId,
            message.ChatId,
            message.Id,
            user.FirstName,
            message.Content,
            user.UserNameColor,
            attachmentUrl,
            profilePictureUrl,
            replyToMessage?.Content,
            replyToMessage?.User.FirstName,
            replyToMessage?.User.UserNameColor,
            message.CreatedAt,
            message.UpdatedAt);

        await hubContext.Clients.Group(message.ChatId.ToString()).SendMessage(response);
        return response;
    }

    public async Task<Response<MessageResponse>> DeleteMessage(Guid messageId)
    {
        var message = await context.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.UserId == httpContextProvider.CurrentUserId);

        if (message is null)
        {
            return ResponseErrors.MessageNotFound;
        }

        var isUserMemberOfChat = await IsUserMemberOfChat(message.ChatId);

        if (!isUserMemberOfChat)
        {
            return ResponseErrors.NotMemberOfChat;
        }

        if (message.UserId != httpContextProvider.CurrentUserId)
        {
            return ResponseErrors.NoRightsForAction;
        }

        context.Messages.Remove(message);
        await context.SaveChangesAsync();

        return Response<MessageResponse>.Success();
    }


    public async Task<Response<IEnumerable<MessageResponse>>> GetMessages(Guid chatId)
    {
        return await context.Messages
            .Where(m => m.ChatId == chatId)
            .Select(message => new MessageResponse(message.UserId,
                message.ChatId,
                message.Id,
                message.User.FirstName,
                message.Content,
                message.User.UserNameColor,
                message.AttachmentFileName == null 
                    ? null 
                    : $"{blobStorageSettings.BlobAccessUrl}/{message.AttachmentFileName}",
                $"{blobStorageSettings.BlobAccessUrl}/{message.User.ProfilePictureFileName}",
                message.ReplyToMessage!.Content,
                message.ReplyToMessage.User.FirstName,
                message.ReplyToMessage.User.UserNameColor,
                message.CreatedAt,
                message.UpdatedAt)).ToListAsync();
    }
    
    private async Task<string?> UploadAttachmentAsync(SendMessageRequest request)
    {
        if (request.FileAttachment is null)
        {
            return null;
        }
        
        var uniqueFileName = FileNameHelper.CreateUniqueFileName(request.FileAttachment.FileName);
        
        await blobService.UploadFileBlobAsync(request.FileAttachment.OpenReadStream(), request.FileAttachment.ContentType,
            uniqueFileName);
        return uniqueFileName;
    }
    
    private async Task<bool> IsUserMemberOfChat(Guid chatId)
    {
        return await context.UserChats
            .AnyAsync(cm => cm.ChatId == chatId && cm.UserId == httpContextProvider.CurrentUserId);
    }
}