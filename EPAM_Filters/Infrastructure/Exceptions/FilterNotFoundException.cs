using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_Filters.Infrastructure.Exceptions
{
    class FilterNotFoundException : Exception
    {
        public string FilterName { get; private set; }

        public FilterNotFoundException(string filterName, string msg) : base(msg)
        {
            FilterName = filterName;
        }
    }
}
