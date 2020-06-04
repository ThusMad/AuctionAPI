using System.Collections.Generic;
using System.Linq;

namespace EPAM_BusinessLogicLayer.BusinessModels
{
    public class Fee
    {
        public decimal Value { get; }

        public decimal GetFeePrice(IEnumerable<string> roles)
        {
            if (roles.Contains(Roles.Premium))
            {
                return Value * 1.025M;
            }
            else {
                return Value * 1.05M;
            }
        }

        public Fee(decimal val)
        {
            Value = val;
        }
    }
}
