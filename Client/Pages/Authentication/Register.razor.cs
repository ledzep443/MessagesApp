using Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;


namespace Client.Pages.Authentication
{
    public partial class Register
    {
        private SignUpRequestDTO SignUpRequest { get; set; } = new SignUpRequestDTO();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        [Inject]
        public IAuthenticationService _authService { get; set; } 
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public Register()
        {
            
        }

        public Register(SignUpRequestDTO signUpRequest, IAuthenticationService authService, NavigationManager navigationManager)
        {
            SignUpRequest = signUpRequest;
         
            _authService = authService;
            _navigationManager = navigationManager;
        }


        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;
            SignUpRequest.HireDate = DateTime.Today.ToString();
            SignUpRequest.Role = "User";
            
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
                ShowRegistrationErrors = true;
            }
            IsProcessing = false;
        }
        

    }
}
