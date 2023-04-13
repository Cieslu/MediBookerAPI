#nullable disable
using MediBookerAPI.Data;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediBookerAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TokenService(IConfiguration configuration, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateBearerToken(string userId)
        {
            var expiry = DateTimeOffset.Now.AddMinutes(15);
            IEnumerable<Claim> userClaims = await GetClaimsForUser(userId);
            return CreateToken(expiry, userClaims);
        }

        public async Task<string> GenerateRefreshToken(string userId)
        {
            var expiry = DateTimeOffset.Now.AddDays(30);
            IEnumerable<Claim> userClaims = await GetClaimsForUser(userId);
            return CreateToken(expiry, userClaims);
        }

        public string CreateToken(DateTimeOffset expiryDate, IEnumerable<Claim> claims)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("TokenSettings:TokenKey").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiryDate.DateTime,
                audience: "https://localhost:44379/",
                issuer: "https://localhost:44379/",
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private async Task<IEnumerable<Claim>> GetClaimsForUser(string userId)
        {
            User user = _userManager.FindByIdAsync(userId).Result;
            if(user != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
                ICollection<string> roles = await _userManager.GetRolesAsync(user);
                claims.Add(new Claim(ClaimTypes.Role, roles.First()));
                return claims;
            }
            else
            {
                return null;
            }
        }
    }
}
