using DataAccess;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace API.Chat
{
    [SignalRHub]
    public class ChatHub : Hub
    {
        public async Task SendAllMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendGroupMessage(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage(ChatMessage message, string userName)
        {
            await Clients.All.SendAsync("ReceivePrivateMessage", message, userName);
        }
        public async Task ChatNotificationAsync(string message, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, receiverUserId, senderUserId);
        }
    }
}
