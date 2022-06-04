using Client.Service;
using Client.Handlers;
using DataAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Models;

namespace Client.Pages.RealtimeChat
{
    public partial class PrivateChat
    {
        [CascadingParameter] public HubConnection hubConnection { get; set; }
        //[CascadingParameter] public AuthStateProvider _authStateProvider { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public string CurrentUserId { get; set; }
        [Parameter] public string CurrentUserEmail { get; set; }
        private List<ChatMessageDTO> messages = new();
        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(ContactEmail))
            {
                var chatHistory = new ChatMessageDTO()
                {
                    Message = CurrentMessage,
                    ToUserId = ContactId,
                    CreatedDate = DateTime.Now.ToString("dd MM yyyy hh:mm tt"),
                };
                await _chatManager.SaveMessageAsync(chatHistory, "private");
                chatHistory.FromUserId = CurrentUserId;
                await hubConnection.SendAsync("SendPrivateMessage", chatHistory, CurrentUserEmail);
                CurrentMessage = string.Empty;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if(hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder().WithUrl(new Uri("https://localhost:7193/chatHub"), options => { 
                    options.HttpMessageHandlerFactory = innerHandler => new IncludeRequestCredentialsHandler { InnerHandler = innerHandler };
                }).Build();
            }
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
            hubConnection.On<ChatMessageDTO, string>("ReceivePrivateMessage", async (message, userName) =>
            {
                if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {
                    if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                    {
                        messages.Add(new ChatMessageDTO { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = CurrentUserEmail } });
                        await hubConnection.SendAsync("ChatNotificationAsync", $"New message from {userName}", ContactId, CurrentUserId);
                    }
                    else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                    {
                        messages.Add(new ChatMessageDTO { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = ContactEmail } });
                    }
                    await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                    StateHasChanged();
                }
            });
            await GetUsersAsync();
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            CurrentUserId = user.Claims.Where(a => a.Type == "Id").Select(a => a.Value).FirstOrDefault();
            CurrentUserEmail = user.Claims.Where(a => a.Type == "name").Select(a => a.Value).FirstOrDefault();
            Console.WriteLine($"{ContactEmail}, {CurrentUserEmail}");
            if (!string.IsNullOrEmpty(ContactEmail))
            {
                await LoadUserChat(ContactEmail);
            }
        }

        public List<ApplicationUser> ChatUsers = new();
        [Parameter] public string ContactEmail { get; set; }
        [Parameter] public string ContactId { get; set; }
        async Task LoadUserChat(string userName)
        {
            var contact =  await _chatManager.GetUserDetailsAsync(userName);
            ContactId = contact.Id;
            ContactEmail = contact.Email;
            _navigationManager.NavigateTo($"privateChat/{ContactEmail}");
            messages = new List<ChatMessageDTO>();
            messages = await _chatManager.GetPrivateChatAsync(ContactId);
        }
        private async Task GetUsersAsync()
        {
            ChatUsers = await _chatManager.GetUsersAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }
    }
}
