using Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System.Web;

namespace Client.Pages.Authentication
{
    public partial class Register
    {
        private SignUpRequestDTO SignUpRequest { get; set; } = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegisterErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

        [Inject]
        public IAuthenticationService _authService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        private async Task RegisterUser()
        {
            ShowRegisterErrors = false;
            IsProcessing = true;
            var result = await _authService.SignUp(SignUpRequest);
            if (result.IsRegistrationSuccessful)
            {
                //Registration is successful, redirect to login
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                //Registration is not successful, show errors
                Errors = result.Errors;
                ShowRegisterErrors = true;
            }
            IsProcessing = false;
        }
        

    }
}
