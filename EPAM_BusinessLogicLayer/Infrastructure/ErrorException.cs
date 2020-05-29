using System;

namespace EPAM_BusinessLogicLayer.Infrastructure
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