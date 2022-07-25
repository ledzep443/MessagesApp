using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Business.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatRepository(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<ChatMessage>> GetPublicChat()
        {
            var messages = await _context.ChatMessages
                .Where(x => x.RoomName == "public")
                .OrderBy(x => x.CreatedDate)
                //.Include(x => x.FromUser)
                .Select(x => new ChatMessageDTO
                {
                    FromUserId = x.FromUserId,
                    FromUser = x.FromUser,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    RoomName = x.RoomName
                }).ToListAsync();
            var recentMessages = new List<ChatMessageDTO>();
            foreach (var message in messages)
            {
                if (DateTime.ParseExact(message.CreatedDate, "dd MM yyyy HH:mm tt", CultureInfo.CurrentCulture) >
                    DateTime.Now.AddHours(-1))
                {
                    recentMessages.Add(message);
                }
            }
            //var recentMessages = messages.FindAll(message => DateTime.ParseExact(message.CreatedDate, "dd MM yyyy HH:mm tt", CultureInfo.InvariantCulture) > DateTime.Now.AddHours(-1));
            var mappedMessages = _mapper.Map<IEnumerable<ChatMessageDTO>, IEnumerable<ChatMessage>>(recentMessages);
            return mappedMessages;
        }

        public async Task<IEnumerable<ChatMessage>> GetPrivateChat(string contactId)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToString();
            var messages = await _context.ChatMessages
                .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
                .Where(h => h.RoomName == "private")
                .OrderBy(a => a.CreatedDate)
                .Include(a => a.FromUser)
                .Include(a => a.ToUser)
                .Select(x => new ChatMessageDTO
                {
                    FromUserId = x.FromUserId,
                    Message = x.Message,
                    RoomName = x.RoomName,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ToUserId = x.ToUserId,
                    ToUser = x.ToUser,
                    FromUser = x.FromUser,
                }).ToListAsync();
            var mappedMessages = _mapper.Map<IEnumerable<ChatMessageDTO>, IEnumerable<ChatMessage>>(messages);
            return mappedMessages;
        }

        public async Task<ChatMessageDTO> SavePublicChat(ChatMessageDTO message)
        {
            try
            {
                var messageObj = _mapper.Map<ChatMessageDTO, ChatMessage>(message);
                var userId = _httpContextAccessor.HttpContext.User.Claims.Where(a => a.Type == "Id").Select(a => a.Value).FirstOrDefault();
                Console.WriteLine(userId);
                messageObj.FromUserId = userId;
                messageObj.CreatedDate = DateTime.Now.ToString("dd MM yyyy HH:mm tt");
                messageObj.RoomName = "public";
                var addedMessageObj = await _context.ChatMessages.AddAsync(messageObj);
                await _context.SaveChangesAsync();
                return _mapper.Map<ChatMessage, ChatMessageDTO>(addedMessageObj.Entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatMessageDTO> SavePrivateChat(ChatMessageDTO message)
        {
            try
            {
                var messageObj = _mapper.Map<ChatMessageDTO, ChatMessage>(message);
                var userId = _httpContextAccessor.HttpContext.User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
                messageObj.FromUserId = userId;
                messageObj.CreatedDate = DateTime.Now.ToString("dd MM yyyy HH:mm tt");
                messageObj.RoomName = "private";
                messageObj.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
                var addedMessageObj = await _context.ChatMessages.AddAsync(messageObj);
                await _context.SaveChangesAsync();
                return _mapper.Map<ChatMessage, ChatMessageDTO>(addedMessageObj.Entity);
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
