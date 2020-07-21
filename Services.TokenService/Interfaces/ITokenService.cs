using System;
using System.Threading.Tasks;

namespace Services.TokenService.Interfaces
{
    public interface ITokenService
    {
        Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, int lifetime = 5);
        Task UpdateRefreshTokenAsync(string username, string refreshToken, int lifetime = 5);
        Task RemoveTokenFromUserAsync(Guid userId);
        Task<bool> CheckTokenIdentityAsync(Guid userId, string refreshToken);
    }
}