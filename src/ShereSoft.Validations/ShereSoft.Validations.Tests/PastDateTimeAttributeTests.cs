using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class PastDateTimeAttributeTests
    {
        [Fact]
        public void IsValid_ReturnsTrueWhenValueIsPastDate()
        {
            var attr = new PastDateTimeAttribute();

            Assert.True(attr.IsValid(DateTime.UtcNow.AddDays(-1)));
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsNull()
        {
            var attr = new PastDateTimeAttribute();
            var model = Tuple.Create((DateTime?)null);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsPastDate()
        {
            var attr = new PastDateTimeAttribute();
            var model = Tuple.Create(DateTime.UtcNow.AddDays(-1));

            var context = new ValidationContext(model);
            context.MemberName = nameof(model.Item1);

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsCustomErrorMessageWhenValueIsPastDate()
        {
            var attr = new PastDateTimeAttribute { ErrorMessage = "ERROR" };
            var model = Tuple.Create(DateTime.UtcNow.AddDays(1));
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);
            Assert.Equal(attr.ErrorMessage, result.ErrorMessage);
        }

        [Fact]
        public void GetValidationResult_ReturnsExternalCustomErrorMessage()
        {
            var attr = new PastDateTimeAttribute();
            attr.ErrorMessageResourceType = typeof(ErrorMessages);
            attr.ErrorMessageResourceName = nameof(ErrorMessages.ValueMustNotBeFutureDate);

            var model = Tuple.Create(DateTime.UtcNow.AddDays(1));
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);
            Assert.Equal(String.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueMustNotBeFutureDate, nameof(model.Item1)), result.ErrorMessage);
        }

        class ErrorMessages
        {
            public static string ValueMustNotBeFutureDate { get; } = "{0} cannot be a future date.";
        }
    }
}
