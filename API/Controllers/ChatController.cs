using Business.Repository.IRepository;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        public ChatController(IUserRepository userRepository, IChatRepository chatRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var allUsers = await _userRepository.GetUsersAsync();
            return Ok(allUsers);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user = await _userRepository.GetUserDetailsAsync(userId);
            return Ok(user);
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetPrivateConversationAsync(string contactId)
        {
            var messages = await _chatRepository.GetPrivateChat(contactId);
            return Ok(messages);
        }
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicConversationAsync()
        {
            var messages = await _chatRepository.GetPublicChat();
            return Ok(messages);
        }

        [HttpPost("private")]
        public async Task<IActionResult> SavePrivateChatAsync(ChatMessageDTO message)
        {
            var savedMessage = await _chatRepository.SavePrivateChat(message);
            return Ok(savedMessage);
        }

        [HttpPost("public")]
        public async Task<IActionResult> SavePublicChatAsync(ChatMessageDTO message)
        {
            var savedMessage = await _chatRepository.SavePublicChat(message);
            return Ok(savedMessage);
        }
    }
}
