using System;
using System.Threading.Tasks;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface ITokenService
    {
        Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, int lifetime = 5);
        Task UpdateRefreshTokenAsync(string username, string refreshToken, int lifetime = 5);
        Task RemoveTokenFromUserAsync(Guid userId);
        bool CheckTokenIdentity(Guid userId, string refreshToken);
    }
}