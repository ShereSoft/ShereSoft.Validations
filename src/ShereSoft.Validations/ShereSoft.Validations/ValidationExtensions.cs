using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying an object instance
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Determines whether the specified value is valid per ValidationAttributes and/or IValidatableObject.Validate method
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>A collection of instances of the System.ComponentModel.DataAnnotations.ValidationResult class.</returns>
        public static ICollection<ValidationResult> GetValidationResults(this object obj)
        {
            return GetValidationResults(obj, null, null);
        }

        /// <summary>
        /// Determines whether the specified value is valid per ValidationAttributes and/or IValidatableObject.Validate method
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="serviceProvider"></param>
        /// <returns>A collection of instances of the System.ComponentModel.DataAnnotations.ValidationResult class.</returns>
        public static ICollection<ValidationResult> GetValidationResults(this object obj, IServiceProvider serviceProvider)
        {
            return GetValidationResults(obj, serviceProvider, null);
        }

        /// <summary>
        /// Determines whether the specified value is valid per ValidationAttributes and/or IValidatableObject.Validate method
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="items">An optional set of key/value pairs to make available to consumers. This parameter is required.</param>
        /// <returns>A collection of instances of the System.ComponentModel.DataAnnotations.ValidationResult class.</returns>
        public static ICollection<ValidationResult> GetValidationResults(this object obj, IDictionary<object, object> items)
        {
            return GetValidationResults(obj, null, items);
        }

        /// <summary>
        /// Determines whether the specified value is valid per ValidationAttributes and/or IValidatableObject.Validate method
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="serviceProvider">The object that implements the System.IServiceProvider interface. This parameter is required.</param>
        /// <param name="items"></param>
        /// <returns>A collection of instances of the System.ComponentModel.DataAnnotations.ValidationResult class.</returns>
        public static ICollection<ValidationResult> GetValidationResults(this object obj, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj, serviceProvider, items);

            Validator.TryValidateObject(obj, context, results, true);  // does not invoke IValidatableObject.Validate when attribute validations fail

            if (obj is IValidatableObject valObj)
            {
                var enumerator = valObj.Validate(context).GetEnumerator();

                if (enumerator.MoveNext())
                {
                    // preventing IValidatableObject.Validate from being run twice
                    if (!results.Any(r => r.ErrorMessage == enumerator.Current.ErrorMessage && !r.MemberNames.Except(enumerator.Current.MemberNames).Any()))
                    {
                        results.Add(enumerator.Current);

                        while (enumerator.MoveNext())
                        {
                            results.Add(enumerator.Current);
                        }
                    }
                }
            }

            return results;
        }
    }
}
