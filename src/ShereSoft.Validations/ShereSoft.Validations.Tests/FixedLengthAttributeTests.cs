using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class FixedLengthAttributeTests
    {
        [Fact]
        public void IsValid_ReturnsTrueWhenLengthIsAsSpecified()
        {
            Assert.True(new FixedLengthAttribute(3).IsValid("ABC"));
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenLengthIsAsSpecified()
        {
            var attr = new FixedLengthAttribute(3);
            var model = Tuple.Create("abc");
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsNullWhenValueIsNull()
        {
            var attr = new FixedLengthAttribute(3);
            var model = Tuple.Create((string)null);
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.Null(result);
        }

        [Fact]
        public void GetValidationResult_ReturnsCustomErrorMessage()
        {
            const int length = 3;
            var attr = new FixedLengthAttribute(length);
            attr.ErrorMessage = "{0} is bad";

            var model = Tuple.Create("BTCN");
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);

            var formattedMessage = String.Format(attr.ErrorMessage, nameof(model.Item1), length);
            Assert.Equal(formattedMessage, result.ErrorMessage);
        }

        [Fact]
        public void GetValidationResult_ReturnsExternalCustomErrorMessage()
        {
            const int length = 3;
            var attr = new FixedLengthAttribute(length);
            attr.ErrorMessageResourceType = typeof(ErrorMessages);
            attr.ErrorMessageResourceName = nameof(ErrorMessages.ValueMustBeNLetterCode);

            var model = Tuple.Create("BTCN");
            var context = new ValidationContext(model) { MemberName = nameof(model.Item1) };

            var result = attr.GetValidationResult(model.Item1, context);

            Assert.NotNull(result);
            Assert.Equal(String.Format(ErrorMessages.ValueMustBeNLetterCode, nameof(model.Item1), length), result.ErrorMessage);
        }

        public class ErrorMessages
        {
            public static string ValueMustBeNLetterCode { get; } = "{0} must be a {1} letter code.";
        }
    }
}
