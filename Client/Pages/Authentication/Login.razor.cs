using Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System.Web;

namespace Client.Pages.Authentication
{
    public partial class Login
    {
        private SignInRequestDTO SignInRequest = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowSignInErrors { get; set; }
        public string Errors { get; set; }
        [Inject]
        public IAuthenticationService _authService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        public string ReturnUrl { get; set; }
        public async Task LoginUser()
        {
            ShowSignInErrors = false;
            IsProcessing = true;
            var result = await _authService.SignInUser(SignInRequest);
            if (result.IsSignInSuccessful)
            {
                var absoluteUri = new Uri(_navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];
                if(string.IsNullOrEmpty(ReturnUrl))
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    _navigationManager.NavigateTo("/" + ReturnUrl);
                }
            }
            else
            {
                //If login fails
                ShowSignInErrors = true;
                Errors = result.ErrorMessage;
            }
            IsProcessing = false;
        }
    }
}
