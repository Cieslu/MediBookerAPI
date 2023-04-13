#nullable disable
using MediBookerAPI.ModelsDTO;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MediBookerAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        public AccountService(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<TokenInfoDTO> LoginUser(UserLoginRequestDTO loginData)
        {
            User user = await _userManager.FindByEmailAsync(loginData.Email);
            if (user != null)
            {
                String passwordCompare = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password).ToString();
                if (passwordCompare == "Success" || passwordCompare == "SuccessRehashNeeded")
                {
                    var result = new TokenInfoDTO();
                    result.AccessToken = await _tokenService.GenerateBearerToken(user.Id);
                    result.RefreshToken = await _tokenService.GenerateRefreshToken(user.Id);

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
