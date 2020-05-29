using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM_BusinessLogicLayer.BusinessModels
{
    class Fee
    {
        private decimal _value = 0;
        public decimal Value { get { return _value; } }
        public decimal GetFeePrice(IEnumerable<string> roles)
        {
            if (roles.Contains(Roles.Premium))
            {
                return _value * 1.025M;
            }
            else {
                return _value * 1.05M;
            }
        }

        public Fee(decimal val)
        {
            _value = val;
        }
    }
}
