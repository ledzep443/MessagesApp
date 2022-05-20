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
        public Task<IEnumerable<ChatMessageDTO>> GetChat(string roomName);
        //public Task<IEnumerable<ChatDTO>> GetPrivateChat(string userFrom, string userTo);
        public Task<ChatMessageDTO> SaveChat(string roomName, ChatMessageDTO chat);
        //public Task<bool> SavePrivateChat(string userFrom, string userTo);
    }
}
