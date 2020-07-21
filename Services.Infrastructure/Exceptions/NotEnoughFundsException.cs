using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Infrastructure.Exceptions
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
