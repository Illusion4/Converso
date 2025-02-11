using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Interfaces;

public interface IChatsService
{
    Task<Response<ChatResponse>> CreateChat(CreateChatRequest request);
    
    Task<Response<ChatResponse>> GetChat(Guid chatId);
    
    Task<Response<IEnumerable<ChatResponse>>> SearchChats(string query);
    
    Task<Response<IEnumerable<ChatResponse>>> GetChats();
    
    Task<Response<ChatResponse>> DeleteChat(Guid chatId);
    
    Task<Response<ChatResponse>> JoinChat(JoinChatRequest request);
    
    Task<Response<ChatResponse>> LeaveChat(Guid chatId);
}