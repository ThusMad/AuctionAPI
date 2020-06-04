using System;

namespace Services.Infrastructure.Exceptions
{
    public class NotInjectedException : Exception
    {
        public NotInjectedException(string msg) : base(msg)
        {

        }
    }
}
