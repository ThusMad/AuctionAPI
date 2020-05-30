using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPAM_DataAccessLayer.Infrastructure
{
    /// <summary>
    /// Occurs when something went wrong after calling SaveChanges()
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Collection of validation errors
        /// </summary>
        public List<ValidationResult> EntityValidationErrors { get; }

        /// <summary>
        /// Ctor that initializes new instance of <seealso cref="ValidationResult"/>
        /// </summary>
        /// <param name="entityValidationErrors">List of validation errors</param>
        /// <param name="msg">Typical exception message</param>
        public ValidationException(List<ValidationResult> entityValidationErrors, string msg = "") : base(msg)
        {
            EntityValidationErrors = entityValidationErrors;
        }
    }
}