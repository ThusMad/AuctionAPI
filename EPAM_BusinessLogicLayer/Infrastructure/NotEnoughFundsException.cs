using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class NotEnoughFundsException : ErrorException
    {
        public decimal Current { get; }
        public decimal Wherewithal { get; }

        public NotEnoughFundsException(string msg, decimal current, decimal wherewithal) : base(msg)
        {
            Current = current;
            Wherewithal = wherewithal;
        }
    }
}
