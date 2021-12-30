using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Validates a country code
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CountryCodeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets country codes to be used for validation
        /// </summary>
        public static IEnumerable<string> Countries
        {
            get => CustomDictionary != null ? CustomDictionary.Keys : DefaultDictionary.Keys;
            set
            {
                lock (SyncLock)
                {
                    var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var country in value)
                    {
                        if (!dict.ContainsKey(country))
                        {
                            dict.Add(country, country);
                        }
                    }

                    CustomDictionary = dict.Count > 0 ? dict : null;
                }
            }
        }

        /// <summary>
        /// Determines whether the value gets normalized. The normalized value is a capitalized interned string.
        /// </summary>
        public bool Normaize { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        public override bool RequiresValidationContext => Normaize;

        static Dictionary<string, string> CustomDictionary = null;

        static Dictionary<string, string> DefaultDictionary = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(culture => new RegionInfo(culture.Name))
            .Where(n => n.TwoLetterISORegionName.Length == 2 && n.TwoLetterISORegionName.All(Char.IsLetter))
            .GroupBy(n => Tuple.Create(n.TwoLetterISORegionName, n.EnglishName))
            .Select(t => t.Key.Item1)
            .OrderBy(c => c)
            .ToDictionary(c => c, c => c, StringComparer.OrdinalIgnoreCase);

        static object SyncLock = new object();

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

            return $"The field {0} must be a valid country code.";
        }

        /// <summary>
        /// Determines whether the specified value is a valid country code.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the System.ComponentModel.DataAnnotations.ValidationResult class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value?.ToString();

            if (str != null)
            {
                var dict = CustomDictionary ?? DefaultDictionary;
                var valid = dict.ContainsKey(str);

                if (!valid)
                {
                    var error = String.Format(FormatErrorMessage(validationContext.MemberName), value);

                    return new ValidationResult(error, new[] { validationContext.MemberName });
                }

                if (Normaize)
                {
                    if (validationContext == null)
                    {
                        throw new ArgumentNullException(nameof(validationContext));
                    }

                    var normalized = dict[str];

                    if (normalized != str)
                    {
                        validationContext.ObjectInstance.GetType().GetProperty(validationContext.MemberName).SetValue(validationContext.ObjectInstance, normalized);
                    }
                }
            }

            return null;
        }
    }
}
