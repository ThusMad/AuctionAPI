using System;
using System.ComponentModel.DataAnnotations;

namespace EPAM_BusinessLogicLayer.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class TimestampValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (long)value > Utility.DateTimeToUnixTimestamp(DateTime.UtcNow.AddMinutes(5));
        }
    }
}
