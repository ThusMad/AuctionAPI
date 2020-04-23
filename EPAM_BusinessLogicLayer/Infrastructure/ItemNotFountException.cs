using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class ItemNotFountException : Exception
    {
        public string PropertyName { get; private set; }

        public ItemNotFountException(string prop, string msg) : base(msg)
        {
            PropertyName = prop;
        }
    }
}
