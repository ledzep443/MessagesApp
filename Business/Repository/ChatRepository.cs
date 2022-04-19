using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Models;

namespace Business.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ChatRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatDTO>> GetChat(string roomName)
        {
            var mappedChat = _mapper.Map<IEnumerable<Chat>, IEnumerable<ChatDTO>>(_context.Chat);
            return mappedChat.Where(x => x.RoomName == roomName);
        }

        public async Task<ChatDTO> SaveChat(string roomName, ChatDTO chat)
        {
            try
            {
                var chatToSave = _mapper.Map<ChatDTO, Chat>(chat);
                chatToSave.RoomName = roomName;
                var addedChat = _context.Chat.Add(chatToSave);
                await _context.SaveChangesAsync();
                return _mapper.Map<Chat, ChatDTO>(addedChat.Entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
