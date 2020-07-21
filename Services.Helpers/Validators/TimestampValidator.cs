using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TimestampValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (long)value > DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds();
        }
    }
}
