using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPAM_API.Services.Interfaces
{
    public interface ITokenProvider
    {
        Task<string> GenerateAccessToken(string username);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        string RefreshAccessToken(string expiredAccessToken, out string newRefreshToken);
        Guid GetUserIdFromExpiredToken(string token);
    }
}