using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class UserException : Exception
    {
        public int ErrorCode { get; private set; }

        public UserException(string msg) : base(msg)
        {
            ErrorCode = 200;
        }

        public UserException(int errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }
    }
}
