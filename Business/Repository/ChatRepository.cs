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

        public async Task<IEnumerable<ChatMessageDTO>> GetChat(string roomName)
        {
            var mappedChat = _mapper.Map<IEnumerable<ChatMessage>, IEnumerable<ChatMessageDTO>>(_context.ChatMessages);
            return mappedChat.Where(x => x.RoomName == roomName);
        }

        public async Task<ChatMessageDTO> SaveChat(string roomName, ChatMessageDTO chat)
        {
            try
            {
                var chatToSave = _mapper.Map<ChatMessageDTO, ChatMessage>(chat);
                chatToSave.RoomName = roomName;
                var addedChat = _context.ChatMessages.Add(chatToSave);
                await _context.SaveChangesAsync();
                return _mapper.Map<ChatMessage, ChatMessageDTO>(addedChat.Entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
