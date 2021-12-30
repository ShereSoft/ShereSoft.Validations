using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Specifies that a data field value has a value with the specified length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FixedLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets the required length of the string data.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///  Initializes a new instance of the System.ComponentModel.DataAnnotations.MaxLengthAttribute class based on the length parameter.
        /// </summary>
        /// <param name="length">The required length of the string data.</param>
        public FixedLengthAttribute(int length)
        {
            Length = length;
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage != null)
            {
                return String.Format(CultureInfo.CurrentCulture, ErrorMessage, name, Length);
            }

            if (ErrorMessageResourceType != null && ErrorMessageResourceName != null)
            {
                return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Length);
            }

            return $"The field {0} must be a string with a length of {1}.";
        }

        /// <summary>
        /// Checks that the data field has a string value with the specified length.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string str)
            {
                return str.Length == Length;
            }

            return true;
        }
    }
}
