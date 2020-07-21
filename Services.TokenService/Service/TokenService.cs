using System;
using System.Linq;
using System.Threading.Tasks;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.Exceptions;
using Services.TokenService.Interfaces;

namespace Services.TokenService.Service
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, int lifetime = 5)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            await UpdateTokenAsync(user, refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(string username, string refreshToken, int lifetime = 5)
        {
            var user = await _userManager.FindByNameAsync(username);

            await UpdateTokenAsync(user, refreshToken).ConfigureAwait(false);
        }

        public async Task RemoveTokenFromUserAsync(Guid userId)
        {
            var token = _unitOfWork.Find<RefreshToken>(a => a.UserId == userId.ToString());

            if (token.Any())
            {
                _unitOfWork.Remove(token.First());
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> CheckTokenIdentityAsync(Guid userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException(nameof(userId), "id was null");
            }

            var refreshTokens = await _unitOfWork.Find<RefreshToken>(t => t.Token == refreshToken).ToListAsync();

            if (!refreshTokens.Any())
            {
                throw new ItemNotFountException("User", $"User with following {nameof(refreshToken)} not found");
            }

            if (refreshTokens.First().TokenExpiration < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                throw new RefreshTokenExpireException(refreshTokens.First(), "Current token expired");
            }

            return refreshTokens.First().UserId == userId.ToString();
        }

        private async Task UpdateTokenAsync(ApplicationUser user, string newRefreshToken)
        {
            var refreshTokens = _unitOfWork.Find<RefreshToken>(a => a.UserId == user.Id).AsEnumerable();
            var tokens = refreshTokens as RefreshToken[] ??
                         (refreshTokens ?? throw new ArgumentNullException(nameof(refreshTokens))).ToArray();

            if (!tokens.Any())
            {
                await _unitOfWork.InsertAsync(new RefreshToken
                {
                    Token = newRefreshToken,
                    User = user,
                    UserId = user.Id,
                    TokenExpiration = DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds()
                });
            }
            else
            {
                var refreshTokenEntity = tokens.First();
                refreshTokenEntity.TokenExpiration =
                    DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds();
                refreshTokenEntity.Token = newRefreshToken;

                _unitOfWork.Update(refreshTokenEntity);
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
