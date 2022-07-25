using Blazored.LocalStorage;
using Client.Service;
using Client.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Models;
using Syncfusion.Blazor.RichTextEditor;
using System.Security.Claims;

namespace Client.Pages.RealtimeChat
{
    public partial class MainChat
    {
        [CascadingParameter] public HubConnection hubConnection { get; set; }
        //[CascadingParameter] public AuthStateProvider _authStateProvider { get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public string CurrentUserId { get; set; }
        [Parameter] public string CurrentUserEmail { get; set; }
        private List<ChatMessageDTO> messages = new();

        protected override async Task OnInitializedAsync()
        {
            if (hubConnection == null)
            {

                hubConnection = new HubConnectionBuilder()
                    .WithUrl(new Uri("https://messagesappapi.azurewebsites.net/chatHub"), options => { 
                        options.HttpMessageHandlerFactory = innerHandler => new IncludeRequestCredentialsHandler { InnerHandler = innerHandler };
                    })
                    .Build();
            }
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
            hubConnection.On<ChatMessageDTO, string>("ReceiveMessage", async (message, userName) =>
            {
                messages.Add(new ChatMessageDTO { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new DataAccess.ApplicationUser() { Email = CurrentUserEmail } });
                await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                StateHasChanged();
            });
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            CurrentUserId = user.Claims.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value.ToString();
            CurrentUserEmail = user.Claims.Where(a => a.Type == "name").Select(a => a.Value).FirstOrDefault();


            await LoadChat();
            
        }
        
        async Task LoadChat()
        {
            messages = new List<ChatMessageDTO>();
            messages = await _chatManager.GetPublicChatAsync();
            StateHasChanged();
        }

        private async Task SendMessage()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            CurrentUserId = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            CurrentUserEmail = user.Claims.Where(a => a.Type == "name").Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(CurrentMessage))
            {
                var chatHistory = new ChatMessageDTO()
                {
                    FromUserId = CurrentUserId,
                    Message = CurrentMessage,
                    CreatedDate = DateTime.Now.ToString("dd MM yyyy HH:mm tt"),
                };
                Console.WriteLine(CurrentUserId);
                
                await _chatManager.SaveMessageAsync(chatHistory, "public");
                await hubConnection.SendAsync("SendAllMessageAsync", chatHistory);
                messages = await _chatManager.GetPublicChatAsync();
                CurrentMessage = string.Empty;
                StateHasChanged();
            }
            
            //await _jsRuntime.InvokeVoidAsync("ScrollToBottom", "chatContainer");
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeVoidAsync("ScrollToBottom", "chatContainer");
            
        }

        
    }

}

