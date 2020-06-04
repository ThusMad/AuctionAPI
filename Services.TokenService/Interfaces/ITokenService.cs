using System;
using System.Threading.Tasks;

namespace Services.TokenService.Interfaces
{
    public interface ITokenService
    {
        Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, int lifetime = 5);
        Task UpdateRefreshTokenAsync(string username, string refreshToken, int lifetime = 5);
        Task RemoveTokenFromUserAsync(Guid userId);
        bool CheckTokenIdentity(Guid userId, string refreshToken);
    }
}