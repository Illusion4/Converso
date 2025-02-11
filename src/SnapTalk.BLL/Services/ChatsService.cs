using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SnapTalk.BLL.Helpers;
using SnapTalk.BLL.Hubs;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;
using SnapTalk.Domain.Constants;
using SnapTalk.Domain.Context;
using SnapTalk.Domain.Entities;
using SnapTalk.Domain.Entities.Enums;

namespace SnapTalk.BLL.Services;

public class ChatsService(SnapTalkContext context,
    IHubContext<ChatHub, IHubClient> hubContext,
    ICurrentContextProvider httpContextProvider,
    IAvatarService avatarService,
    BlobStorageSettings blobStorageSettings,
    IBlobService blobService) : IChatsService
{

    public async Task<Response<ChatResponse>> CreateChat(CreateChatRequest request)
    {
        var avatarBytes = avatarService.GenerateAvatar(request.Title, UserNameColors.GetRandomColor());
        var uniqueAvatarFileName = FileNameHelper.CreateUniqueFileName($"{request.Title}.png");
        await blobService.UploadFileBlobAsync(new MemoryStream(avatarBytes), "image/png", uniqueAvatarFileName);
        
        var chat = new ChatEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Title,
            Description = request.Description,
            ImageFileName = uniqueAvatarFileName,
            ChatType = ChatType.PublicChat
        };

        var userChat = new UserChatEntity
        {
            UserId = httpContextProvider.CurrentUserId,
            ChatId = chat.Id,
            Role = UserChatRole.Owner
        };

        context.Chats.Add(chat);

        context.UserChats.Add(userChat);

        await context.SaveChangesAsync();

        return new ChatResponse(chat.Id,
            chat.Name,
            chat.Description,
            chat.ChatType,
            1,
            true,
            String.Empty,
            null,
            String.Empty,
            $"{blobStorageSettings.BlobAccessUrl}/{uniqueAvatarFileName}");
    }

    public async Task<Response<ChatResponse>> GetChat(Guid chatId)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<IEnumerable<ChatResponse>>> SearchChats(string query)
    {
        return await (
            from x in context.Chats
            where x.Name.Contains(query)
            from lastMessage in x.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Take(1)
                .DefaultIfEmpty()
            select new ChatResponse(
                x.Id,
                x.Name,
                x.Description,
                x.ChatType,
                x.UserChats.Count(),
                x.UserChats.Any(uc => uc.UserId == httpContextProvider.CurrentUserId),
                lastMessage.Content,
                lastMessage.CreatedAt,
                $"{lastMessage.User.FirstName}",
                $"{blobStorageSettings.BlobAccessUrl}/{x.ImageFileName}"
            )).ToListAsync();
    }

    public async Task<Response<IEnumerable<ChatResponse>>> GetChats()
    {
        return await (
            from uc in context.UserChats
            where uc.UserId == httpContextProvider.CurrentUserId
            join x in context.Chats on uc.ChatId equals x.Id
            from lastMessage in x.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Take(1)
                .DefaultIfEmpty()
            select new ChatResponse(
                x.Id,
                x.Name,
                x.Description,
                x.ChatType,
                x.UserChats.Count(),
                true,
                lastMessage.Content,
                lastMessage.CreatedAt,
                lastMessage.User.FirstName,
                $"{blobStorageSettings.BlobAccessUrl}/{x.ImageFileName}"
            )).ToListAsync();
    }

    public async Task<Response<ChatResponse>> DeleteChat(Guid chatId)
    {
        var chat = await context.Chats.FirstOrDefaultAsync(x => x.Id == chatId
                               && x.UserChats.Any(uc => uc.UserId == httpContextProvider.CurrentUserId
                                                         && uc.Role == UserChatRole.Owner));

        if (chat == null)
        {
            return ResponseErrors.CannotDeleteChatNotOwner;
        }
        
        context.Chats.Remove(chat);
        await context.SaveChangesAsync();
        
        return Response<ChatResponse>.Success();
    }

    public async Task<Response<ChatResponse>> JoinChat(JoinChatRequest request)
    {
        var userIsMember = await context.UserChats
            .AnyAsync(uc => uc.ChatId == request.ChatId && uc.UserId == httpContextProvider.CurrentUserId);
        if (userIsMember)
        {
            return ResponseErrors.UserAlreadyInChat;
        }
        
        var chat = await context.Chats.FirstOrDefaultAsync(x => x.Id == request.ChatId
                                                                && x.ChatType == ChatType.PublicChat);
        
        if (chat == null)
        {
            return ResponseErrors.ChatNotFound;
        }
        
        var userChat = new UserChatEntity
        {
            UserId = httpContextProvider.CurrentUserId,
            ChatId = chat.Id,
            Role = UserChatRole.Member
        };
        
        context.UserChats.Add(userChat);
        await context.SaveChangesAsync();
        
        return Response<ChatResponse>.Success();
    }

    public async Task<Response<ChatResponse>> LeaveChat(Guid chatId)
    {
        var userChat = context.UserChats
            .FirstOrDefault(uc => uc.ChatId == chatId && uc.UserId == httpContextProvider.CurrentUserId);

        if (userChat == null)
        {
            return ResponseErrors.NotMemberOfChat;
        }
        
        context.UserChats.Remove(userChat);
        await context.SaveChangesAsync();
        
        return Response<ChatResponse>.Success();
    }
    
}