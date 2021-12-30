using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Specifies that a data field value has at least one element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ElementRequiredAttribute : RequiredAttribute
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

            return $"The field {0} requires at least one value.";
        }

        /// <summary>
        /// Checks that the array or collection of the data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value is IList list)
                {
                    return list.Count > 0;
                }
                else if (value is ICollection collection)
                {
                    return collection.Count > 0;
                }
                else if (value is IEnumerable e)
                {
                    return e.GetEnumerator().MoveNext();
                }

                return base.IsValid(value);
            }

            return false;
        }
    }
}
