using System;

namespace Services.Infrastructure.Exceptions
{
    public abstract class ErrorException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        protected ErrorException(string msg, int errorCode = 200) : base(msg)
        {
            ErrorCode = errorCode;
        }
    }
}