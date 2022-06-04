using DataAccess;
using Models;

namespace Client.Manager
{
    public interface IChatManager
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task<List<ChatMessageDTO>> GetPublicChatAsync();
        Task<List<ChatMessageDTO>> GetPrivateChatAsync(string contactId);
        Task SaveMessageAsync(ChatMessageDTO message, string roomName);
    }
}
