using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediBookerAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateBearerToken(string userId);
        public Task<string> GenerateRefreshToken(string userId);
        public string CreateToken(DateTimeOffset expiryDate, IEnumerable<Claim> claims);
    }
}
