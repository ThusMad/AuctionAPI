using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    internal class AccessViolationException : ErrorException
    {
        public AccessViolationException(string msg) : base(msg)
        {

        }
    }
}
