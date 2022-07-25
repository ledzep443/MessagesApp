using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequest)
        {
            if (signUpRequest == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = signUpRequest.Email,
                Email = signUpRequest.Email,
                Name = signUpRequest.Name,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, signUpRequest.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegistrationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }

            var roleExists = _roleManager.RoleExistsAsync(signUpRequest.Role).Result;
            
            if(!roleExists)
            {
                var role = new IdentityRole
                {
                    Name = signUpRequest.Role
                };
                await _roleManager.CreateAsync(role);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, signUpRequest.Role);
            if (!roleResult.Succeeded)
            {

                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegistrationSuccessful = false,
                    Errors = roleResult.Errors.Select(u => u.Description)
                });
            }
            else
            {
                return Ok(new SignUpResponseDTO()
                {
                    IsRegistrationSuccessful = true
                });
            }


            return StatusCode(201);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequest)
        {
            if (signInRequest == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(signInRequest.UserName, signInRequest.Password, false, false);
            if(result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInRequest.UserName);
                if (user == null)
                {
                    return Unauthorized(new SignInResponseDTO
                    {
                        IsSignInSuccessful = false,
                        Error = "Invalid authentication"
                    });
                }

                //If user is found and authentication succeeds, login
                var signInCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7193",
                    audience: "https://localhost:7158",
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: signInCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new SignInResponseDTO()
                {
                    IsSignInSuccessful = true,
                    Token = token,
                    UserDTO = new UserDTO()
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Email = user.Email,
                        HireDate = user.HireDate,
                        Role = "Developer"
                    }
                });
            }
            else
            {
                return Unauthorized(new SignInResponseDTO
                {
                    IsSignInSuccessful = false,
                    Error = "Invalid Authentication"
                });
            }

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            if (changePasswordRequest == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new ChangePasswordResponseDTO
                {
                    IsPasswordSuccessfullyChanged = true,
                });
            }
            else
            {
                return BadRequest(new ChangePasswordResponseDTO
                {
                    IsPasswordSuccessfullyChanged = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(DeleteAccountRequestDTO deleteAccountRequestDTO)
        {
            if (deleteAccountRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var userToDelete = await _userManager.FindByIdAsync(deleteAccountRequestDTO.UserId);
            if (userToDelete == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(userToDelete);
            if (result.Succeeded)
            {
                return Ok(new DeleteAccountResponseDTO
                {
                    IsAccountSuccessfullyDeleted = true,
                });
            }
            else
            {
                return BadRequest(new DeleteAccountResponseDTO
                {
                    IsAccountSuccessfullyDeleted = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
        }
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["APIKey"]));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            };

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
