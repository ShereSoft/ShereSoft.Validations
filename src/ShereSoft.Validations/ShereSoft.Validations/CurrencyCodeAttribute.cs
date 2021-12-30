using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace ShereSoft.Validations
{
    /// <summary>
    /// Validates a currency code
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CurrencyCodeAttribute : ValidationAttribute
    {
        public static IEnumerable<string> Currencies
        {
            get => CustomDictionary != null ? CustomDictionary.Keys : DefaultDictionary.Keys;
            set
            {
                lock (SyncLock)
                {
                    var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var currency in value)
                    {
                        if (!dict.ContainsKey(currency))
                        {
                            dict.Add(currency, currency);
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

        static Dictionary<string, string> DefaultDictionary = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(c => c.Name != "" ? Tuple.Create(new RegionInfo(c.Name).ISOCurrencySymbol, new RegionInfo(c.Name).CurrencyEnglishName) : Tuple.Create((string)null, (string)null))
                .Where(c => c.Item1 != null)
                .GroupBy(c => c.Item1.ToUpper())
                .ToDictionary(c => c.Key, c => c.Key, StringComparer.OrdinalIgnoreCase);

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

            return $"The field {0} must be a valid currency code.";
        }

        /// <summary>
        /// Determines whether the specified value is a valid currency code.
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
