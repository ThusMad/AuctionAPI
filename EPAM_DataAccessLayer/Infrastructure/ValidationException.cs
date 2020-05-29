using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPAM_DataAccessLayer.Infrastructure
{
    public class ValidationException : Exception
    {
        public List<ValidationResult> EntityValidationErrors { get; }

        public ValidationException(List<ValidationResult> entityValidationErrors, string msg = "") : base(msg)
        {
            EntityValidationErrors = entityValidationErrors;
        }
    }
}