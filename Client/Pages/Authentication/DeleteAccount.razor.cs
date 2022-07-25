using Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;

namespace Client.Pages.Authentication
{
    public partial class DeleteAccount
    {
        private DeleteAccountRequestDTO deleteAccountRequestDTO = new();
        private bool IsProcessing = false;
        private bool ShowDeleteAccountErrors { get; set; }
        private bool ConfirmDeleteAccount { get; set; } = false;
        private IEnumerable<string> Errors { get; set; }
        private string CurrentUserId { get; set; }
        [Inject]
        public IAuthenticationService _authService { get; set; }

        private async Task DeleteUserAccount()
        {
            ShowDeleteAccountErrors = false;
            IsProcessing = true;
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            CurrentUserId = user?.Claims?.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value;
            deleteAccountRequestDTO.UserId = CurrentUserId;
            var result = await _authService.DeleteAccount(deleteAccountRequestDTO);
            if (result.IsAccountSuccessfullyDeleted)
            {
                //Account successfully deleted
                _navigationManager.NavigateTo("/");
            }
            else
            {
                //Account deletion not successful, show errors
                Errors = result.Errors;
                ShowDeleteAccountErrors = true;
            }
            IsProcessing = false;
        }
    }
}
