using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EPAM_BusinessLogicLayer.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    class TimestampValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (long)value > Utility.DateTimeToUnixTimestamp(DateTime.UtcNow.AddMinutes(5));
        }
    }
}
