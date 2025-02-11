using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Interfaces;

public interface IMessageService
{
    Task<Response<MessageResponse>> SendMessage(SendMessageRequest request);
    
    Task<Response<MessageResponse>> DeleteMessage(Guid messageId);
    
    Task<Response<IEnumerable<MessageResponse>>> GetMessages(Guid chatId);
}