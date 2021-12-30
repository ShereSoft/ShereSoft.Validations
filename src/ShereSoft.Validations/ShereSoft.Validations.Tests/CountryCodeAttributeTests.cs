using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class CountryCodeAttributeTests
    {
        [Fact]
        public void IsValid_ReturnsNullWhenValueIsValidCountryCode()
        {
            var attr = new CountryCodeAttribute();
            
            Assert.True(attr.IsValid("US"));
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsNull()
        {
            var attr = new CountryCodeAttribute();
            var model = Tuple.Create((string)null);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void IsValid_NormalizesValue()
        {
            var attr = new CountryCodeAttribute();
            attr.Normaize = true;

            var model = new Model { Value = "us" };

            var context = new ValidationContext(model);
            context.MemberName = nameof(model.Value);

            attr.Validate(model.Value, context);

            Assert.Equal("US", model.Value);
        }

        [Fact]
        public void RequiresValidationContext_ReturnsTrue_WhenNomarlizationIsEnabled()
        {
            var attr = new CountryCodeAttribute();
            attr.Normaize = true;

            Assert.True(attr.RequiresValidationContext);
        }

        [Fact]
        public void IsValid_DoesNotReturnNormalizedValue_WhenNormalizationIsOff()
        {
            var attr = new CountryCodeAttribute();
            attr.Normaize = false;

            var model = Tuple.Create("us");

            var context = new ValidationContext(model);
            context.MemberName = nameof(model.Item1);

            attr.Validate(model.Item1, context);

            Assert.Equal("us", model.Item1);
        }

        [Fact]
        public void CountryCodeAttribute_CanUseCustomCountries()
        {
            try
            {
                CountryCodeAttribute.Countries = new[] { "A1" };

                var attr = new CountryCodeAttribute();

                Assert.True(attr.IsValid("a1"));
            }
            finally
            {
                CountryCodeAttribute.Countries = new string[0];
            }
        }

        class Model
        {
            public string Value { get; set; }
        }
    }
}
