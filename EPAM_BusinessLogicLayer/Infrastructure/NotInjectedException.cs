using System;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class NotInjectedException : Exception
    {
        public NotInjectedException(string msg) : base(msg)
        {

        }
    }
}
