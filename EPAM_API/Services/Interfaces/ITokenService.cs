using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DTO;

namespace EPAM_API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(string username);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        string RefreshAccessToken(string expiredAccessToken, out string newRefreshToken);
        Guid GetUserIdFromExpiredToken(string token);
    }
}