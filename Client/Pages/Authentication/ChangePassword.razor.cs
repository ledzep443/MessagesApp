using Client.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Models;

namespace Client.Pages.Authentication
{
    [Authorize]
    public partial class ChangePassword
    {
        private ChangePasswordRequestDTO ChangePasswordRequest { get; set; }
        public bool IsProcessing { get; set; } = false;
        public bool ShowChangePasswordErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

        [Inject]
        public IAuthenticationService _authService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        private async Task ChangeUserPassword()
        {
            ShowChangePasswordErrors = false;
            IsProcessing = true;
            
            var result = await _authService.ChangePassword(ChangePasswordRequest);
            if (result.IsPasswordSuccessfullyChanged)
            {
                //Password successfully updated
                _navigationManager.NavigateTo("/");
            }
            else
            {
                //Password change not successful, show errors
                Errors = result.Errors;
                ShowChangePasswordErrors = true;
            }
            IsProcessing = false;
        }
    }
}
