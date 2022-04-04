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

        public async Task SendPrivateMessage(string userTo, string userFrom, string message)
        {
            await Clients.User(userTo).SendAsync("ReceiveMessage", userFrom, message);
        }
    }
}
