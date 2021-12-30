using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class CurrencyCodeAttributeTests
    {
        [Fact]
        public void IsValid_ReturnsNullWhenValueIsValidCurrencyCode()
        {
            Assert.True(new CurrencyCodeAttribute().IsValid("USD"));
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsNull()
        {
            var attr = new CurrencyCodeAttribute();
            var model = Tuple.Create((string)null);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void IsValid_NormalizesValue()
        {
            var attr = new CurrencyCodeAttribute { Normaize = true };
            var model = new Model { Value = "usd" };
            var context = new ValidationContext(model) { MemberName = nameof(model.Value) };

            attr.Validate(model.Value, context);

            Assert.Equal("USD", model.Value);
        }

        [Fact]
        public void CurrencyCodeAttribute_CanUseCustomCurrencies()
        {
            try
            {
                CurrencyCodeAttribute.Currencies = new[] { "BTC" };

                var attr = new CurrencyCodeAttribute();

                Assert.True(attr.IsValid("btc"));
            }
            finally
            {
                CurrencyCodeAttribute.Currencies = new string[0];
            }
        }

        class Model
        {
            public string Value { get; set; }
        }
    }
}
