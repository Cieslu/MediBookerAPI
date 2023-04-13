using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Interfaces
{
    public interface IAccountService
    {
        public Task<TokenInfoDTO?> LoginUser(UserLoginRequestDTO loginData);
    }
}
