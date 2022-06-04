using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IChatRepository
    {
        public Task<IEnumerable<ChatMessage>> GetPublicChat();
        public Task<IEnumerable<ChatMessage>> GetPrivateChat(string contactId);
        public Task<ChatMessageDTO> SavePublicChat(ChatMessageDTO message);
        public Task<ChatMessageDTO> SavePrivateChat(ChatMessageDTO message);
    }
}
