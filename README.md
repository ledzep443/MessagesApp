# MessagesApp
This is a real time .NET core 6 application using SignalR to handle real time communication between the Blazor WASM client and ASP.NET core API to 
send messages in a public chat room, or to message other users privately. The client uses MudBlazor components instead of Bootstrap, and this was my first time
using MudBlazor. The API uses JSON Web Tokens for authentication, with a custom JWT handler for SignalR, as well as a custom authentication solution utilizing ASP.NET
core Identity. The messages are all stored in a PostgreSQL database for persistance, so that if a user reloads the page the messages are able to be reloaded and don't
disappear upon page reload.
