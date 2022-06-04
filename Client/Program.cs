using Blazored.LocalStorage;
using Client;
using Client.Manager;
using Client.Service;
using Client.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using MudBlazor.Services;
using MudBlazor;
using Client.Handlers;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjE3MTA2QDMyMzAyZTMxMmUzMGZ6aDBZcEowSmFET2VaZzhIbks3YmJzOGY1VjZuMTh2K0Q0aXJUSzcwYms9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7193") });
builder.Services.AddScoped<IncludeRequestCredentialsHandler>();
//builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri("https://localhost:7193")).AddHttpMessageHandler<IncludeRequestCredentialsHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IChatManager, ChatManager>();

await builder.Build().RunAsync();
