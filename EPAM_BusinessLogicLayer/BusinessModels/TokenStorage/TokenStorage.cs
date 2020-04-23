using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels.TokenStorage.Interfaces;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.BusinessModels.TokenStorage
{
    class TokenStorage : ITokenStorage
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenStorage(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task UpdateRefreshTokenAsync(Guid userId, string refreshToken, int lifetime = 5)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            await UpdateToken(user, refreshToken, lifetime);
        }

        public async Task UpdateRefreshTokenAsync(string username, string refreshToken, int lifetime = 5)
        {
            var user = await _userManager.FindByNameAsync(username);

            await UpdateToken(user, refreshToken, lifetime);
        }

        public async Task RemoveTokenFromUserAsync(Guid userId)
        {
            var token = _unitOfWork.Find<RefreshToken>(a => a.UserId == userId.ToString());

            if (token.Any())
            {
                _unitOfWork.Delete(token.First());
                await _unitOfWork.CommitAsync();
            }
        }

        public bool CheckTokenIdentity(Guid userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException(nameof(userId), "id was null");
            }

            var refreshTokens = _unitOfWork.Repository<RefreshToken>().Find(t => t.Token == refreshToken);
            var tokens = refreshTokens as RefreshToken[] ?? refreshTokens.ToArray();

            if (!tokens.Any())
            {
                throw new ItemNotFountException("User", $"User with following {nameof(refreshToken)} not found");
            }

            if (tokens.First().TokenExpiration < Utility.DateTimeToUnixTimestamp(DateTime.UtcNow))
            {
                throw new RefreshTokenExpireException(tokens.First(), "Current token expired");
            }

            return tokens.First().UserId == userId.ToString();
        }

        private async Task UpdateToken(ApplicationUser user, string newRefreshToken, int lifetime)
        {
            var refreshTokens = _unitOfWork.Find<RefreshToken>(a => a.UserId == user.Id);

            if (refreshTokens != null && !refreshTokens.Any())
            {
                await _unitOfWork.InsertAsync(new RefreshToken()
                {
                    Token = newRefreshToken,
                    User = user,
                    UserId = user.Id,
                    TokenExpiration = Utility.DateTimeToUnixTimestamp(DateTime.UtcNow.AddMinutes(lifetime))
                });
            }
            else
            {
                var refreshTokenEntity = refreshTokens.First();
                refreshTokenEntity.TokenExpiration =
                    Utility.DateTimeToUnixTimestamp(DateTime.UtcNow.AddMinutes(lifetime));
                refreshTokenEntity.Token = newRefreshToken;

                _unitOfWork.Update(refreshTokenEntity);
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
