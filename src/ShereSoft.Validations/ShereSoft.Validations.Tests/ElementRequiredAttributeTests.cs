using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class ElementRequiredAttributeTests
    {
        [Fact]
        public void IsValid_ReturnsTrueWhenMoreThanOneValue()
        {
            Assert.True(new ElementRequiredAttribute().IsValid(new[] { 123 }));
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsNull()
        {
            var attr = new ElementRequiredAttribute();
            var model = Tuple.Create((string[])null);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenMoreThanOneValue()
        {
            var attr = new ElementRequiredAttribute();
            var model = Tuple.Create(new[] { 123});
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsCustomErrorMessage()
        {
            const int length = 3;
            var attr = new ElementRequiredAttribute();
            attr.ErrorMessage = "{0} is bad";

            var model = Tuple.Create(new int[0]);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);

            var formattedMessage = String.Format(attr.ErrorMessage, nameof(model.Item1), length);
            Assert.Equal(formattedMessage, result.ErrorMessage);
        }

        [Fact]
        public void GetValidationResult_ReturnsExternalCustomErrorMessage()
        {
            var attr = new ElementRequiredAttribute();
            attr.ErrorMessageResourceType = typeof(ErrorMessages);
            attr.ErrorMessageResourceName = nameof(ErrorMessages.ValueMustHaveAtLeastOneCode);

            var model = Tuple.Create(new int[0]);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);
            Assert.Equal(String.Format(ErrorMessages.ValueMustHaveAtLeastOneCode, nameof(model.Item1)), result.ErrorMessage);
        }

        public class ErrorMessages
        {
            public static string ValueMustHaveAtLeastOneCode { get; } = "{0} must have at least one code.";
        }
    }
}
