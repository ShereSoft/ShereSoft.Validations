using System;
using System.ComponentModel.DataAnnotations;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Specifies that a data field value has a past DateTime value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PastDateTimeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage != null || (ErrorMessageResourceType != null && ErrorMessageResourceName != null))
            {
                return base.FormatErrorMessage(name);
            }

            return $"The field {0} must be a past date.";
        }

        /// <summary>
        /// Checks that the data field has a past DateTime value.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            if (value != null && value is DateTime d)
            {
                if (d.Kind == DateTimeKind.Utc)
                {
                    return d < DateTime.UtcNow;
                }

                return d.ToUniversalTime() < DateTime.UtcNow;
            }

            return true;
        }
    }
}
