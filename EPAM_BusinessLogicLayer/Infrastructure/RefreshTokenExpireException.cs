using System;
using System.Collections.Generic;
using System.Text;
using EPAM_DataAccessLayer.Entities;

namespace EPAM_BusinessLogicLayer.Infrastructure
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
            TimeElapsed = Utility.DateTimeToUnixTimestamp(DateTime.UtcNow) - ExpirationDate;
        }
    }
}
