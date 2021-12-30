using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShereSoft.Validations.Tests
{
    public class ValidationExtensions
    {
        [Fact]
        public void GetValidationResults_ReturnsErrorResultByAttribute()
        {
            var model = new Model1();

            Assert.Equal(1, model.GetValidationResults().Count);
        }

        [Fact]
        public void GetValidationResults_ReturnsErrorResultByAttributeAndIValidatableValidate()
        {
            var model = new Model2 { Value = "acb" };

            Assert.Equal(2, model.GetValidationResults().Count);
        }

        [Fact]
        public void GetValidationResults_DoesNotReturnDuuplicatedResults()
        {
            var model = new Model3 { Value = "acb" };

            Assert.Equal(1, model.GetValidationResults().Count);
        }

        class Model1
        {
            [Required]
            public string Value { get; set; }
        }

        class Model2 : IValidatableObject
        {
            [RegularExpression("[A-Z]+")]
            public string Value { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Value != null && Value.Length != 2)
                {
                    yield return new ValidationResult("Error");
                }
            }
        }

        class Model3 : IValidatableObject
        {
            [Required]
            public string Value { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Value != null && Value.Length != 2)
                {
                    yield return new ValidationResult("Error");
                }
            }
        }
    }
}
