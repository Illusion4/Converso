using Microsoft.AspNetCore.SignalR;

namespace SnapTalk.BLL.Hubs;

public class ChatHub : Hub<IHubClient>
{
    public Task ConnectToGroup(string groupId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }
}