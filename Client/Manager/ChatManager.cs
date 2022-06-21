
using Blazored.LocalStorage;
using Client.Service;
using DataAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client.Manager
{
    public class ChatManager : IChatManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        public ChatManager(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            await SetAuthenticationHeader();
            var data = await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/Chat/users");
            return data;
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            await SetAuthenticationHeader();
            var result = await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/Chat/users/{userId}");
            return result;
        }

        public async Task<List<ChatMessageDTO>> GetPublicChatAsync()
        {
            await SetAuthenticationHeader();
            return await _httpClient.GetFromJsonAsync<List<ChatMessageDTO>>("api/Chat/public");
        }

        public async Task<List<ChatMessageDTO>> GetPrivateChatAsync(string contactId)
        {
            await SetAuthenticationHeader();
            return await _httpClient.GetFromJsonAsync<List<ChatMessageDTO>>($"api/Chat/{contactId}");
        }

        public async Task SaveMessageAsync(ChatMessageDTO chatMessage, string roomName)
        {
            await SetAuthenticationHeader();
            var user = await GetLocalUserDetailsAsync();
            chatMessage.FromUserId = user.Id;
            await _httpClient.PostAsJsonAsync($"api/Chat/{roomName}", chatMessage);
        }

        private async Task<bool> SetAuthenticationHeader()
        {
            await _authStateProvider.GetAuthenticationStateAsync();
            var token = await _localStorage.GetItemAsync<string>("JWT Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        private async Task<UserDTO> GetLocalUserDetailsAsync()
        {
            return await _localStorage.GetItemAsync<UserDTO>("UserDetails");
        }
    }
}
