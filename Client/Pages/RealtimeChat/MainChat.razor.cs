using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.Blazor.RichTextEditor;


namespace Client.Pages.RealtimeChat
{
    public partial class MainChat : IAsyncDisposable
    {
        private readonly NavigationManager NavigationManager;
        private readonly HttpClient _httpClient;
        
        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }
        private HubConnection? hubConnection;
        private List<DataAccess.ChatMessage> messages = new();
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {


            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7193/chat")
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMessage = $"{user}: {message}";
                //messages.Add(encodedMessage);
                StateHasChanged();
            });

            await hubConnection.StartAsync();

        }

        private async Task SendMessage()
        {
            var authState = await authenticationState;
            var currentUser = authState.User;
            if (hubConnection != null)
            {
                await hubConnection.SendAsync("SendAllMessage", currentUser.Identity.Name, messageInput);
                messageInput = string.Empty;
            }
        }

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }

}

