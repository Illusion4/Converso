using SnapTalk.Common.DTO;

namespace SnapTalk.BLL.Hubs;

public interface IHubClient
{
    Task SendMessage(MessageResponse message);
}