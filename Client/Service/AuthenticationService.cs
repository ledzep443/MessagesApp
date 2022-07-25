using Client.Service.IService;
using Blazored.LocalStorage;
using Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;

        }

        public async Task<SignInResponseDTO> SignInUser(SignInRequestDTO signInRequest)
        {
            var content = JsonConvert.SerializeObject(signInRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signin", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode && result != null)
            {
                await _localStorage.SetItemAsync("JWT Token", result.Token);
                await _localStorage.SetItemAsync("UserDetails", result.UserDTO);
                ((AuthStateProvider)_authStateProvider).NotifyUserLoggedIn(result.Token);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                return new SignInResponseDTO() { IsSignInSuccessful = true };
            }
            else
            {

                return new SignInResponseDTO() { IsSignInSuccessful= false };

            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("JWT Token");
            await _localStorage.RemoveItemAsync("UserDetails");

            ((AuthStateProvider)_authStateProvider).NotifyUserLoggedOut();

            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<ChangePasswordResponseDTO> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            var content = JsonConvert.SerializeObject(changePasswordRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Account/ChangePassword", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChangePasswordResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode && result != null)
            {
                return new ChangePasswordResponseDTO() { IsPasswordSuccessfullyChanged = true };
            }
            else
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return new ChangePasswordResponseDTO()
                {
                    IsPasswordSuccessfullyChanged = false,
                    Errors = result.Errors
                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        public async Task<DeleteAccountResponseDTO> DeleteAccount(DeleteAccountRequestDTO deleteAccountRequest)
        {
            var content = JsonConvert.SerializeObject(deleteAccountRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Account/DeleteAccount", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DeleteAccountResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode && result != null)
            {
                return new DeleteAccountResponseDTO() { IsAccountSuccessfullyDeleted = true };
            }
            else
            {
                return new DeleteAccountResponseDTO()
                {
                    IsAccountSuccessfullyDeleted = false,
                    Errors = result.Errors
                };
            }
            
        }

        public async Task<SignUpResponseDTO> SignUp(SignUpRequestDTO signUpRequest)
        {
            var content = JsonConvert.SerializeObject(signUpRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Account/SignUp", bodyContent);
            var bodyTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignUpResponseDTO>(bodyTemp);

            if (response.IsSuccessStatusCode && result != null)
            {
                return new SignUpResponseDTO { IsRegistrationSuccessful = true };
            }
            else
            {

                return new SignUpResponseDTO { 
                    IsRegistrationSuccessful = false, 
                    /*Errors = result?.Errors*/ };

            }
        }
    }
}
