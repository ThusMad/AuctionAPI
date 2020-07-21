using System;
using EPAM_DataAccessLayer.Entities;

namespace Services.Infrastructure.Exceptions
{
    public class RefreshTokenExpireException : Exception
    {
        public RefreshToken RefreshToken { get; private set; }
        public long ExpirationDate { get; private set; }
        public long TimeElapsed { get; private set; }

        public RefreshTokenExpireException(RefreshToken refreshToken, string msg = default) : base(msg)
        {
            RefreshToken = refreshToken;
            ExpirationDate = refreshToken.TokenExpiration;
            TimeElapsed = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ExpirationDate;
        }
    }
}
