using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class NotInjectedException : Exception
    {
        public NotInjectedException(string msg) : base(msg)
        {

        }
    }
}
