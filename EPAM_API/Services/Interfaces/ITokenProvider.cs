using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DataTransferObjects;

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