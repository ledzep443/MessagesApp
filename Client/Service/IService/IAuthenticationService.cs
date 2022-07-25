using Models;

namespace Client.Service.IService
{
    public interface IAuthenticationService
    {
        Task<SignInResponseDTO> SignInUser(SignInRequestDTO requestDTO);
        Task<SignUpResponseDTO> SignUp(SignUpRequestDTO requestDTO);
        Task<ChangePasswordResponseDTO> ChangePassword(ChangePasswordRequestDTO requestDTO);
        Task<DeleteAccountResponseDTO> DeleteAccount(DeleteAccountRequestDTO requestDTO);
        Task Logout();
    }
}
