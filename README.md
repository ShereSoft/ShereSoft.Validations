# ShereSoft.Validations
Provides common validations extended from the standard ValidationAttribute. No library dependencies.

[![](https://img.shields.io/nuget/v/ShereSoft.Validations.svg)](https://www.nuget.org/packages/ShereSoft.Validations/)
[![](https://img.shields.io/nuget/dt/ShereSoft.Validations)](https://www.nuget.org/packages/ShereSoft.Validations/)

* Light-weight
* Multiple .NET versions (.NET5, .NET Core 3.1, .NET Core 2.1, .NET Framework 4.5)
* Customizable
* No external library dependencies
* No external calls

### CountryCodeAttribute
Checks if the value is a valid 2-digit country code. (i.e. 'US') The country code list can be customized. If Normalization is enabled, the value will be replaced with a capitalized string (interned). The validation succeeds if the value is null.
```csharp
class Model
{
    [CountryCode]
    public string CountryCode { get; set; }
}
```
```csharp
class Model
{
    [CountryCode(Normalized=true)]
    public string CountryCode { get; set; }  // after validated, the value will be replaced with a capitalized value. ('us' becomes 'US') 
}
```
> All Validation Attributes support custom error messages
```csharp
class Model
{
    [CountryCode(ErrorMessage = "'{0}' is not valid for Bill-to Country.")]
    public string BillToCountry { get; set; }
}
```
```csharp
class Model
{
    [CountryCode(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ValueIsNotValidCountryCode")]
    public string BillToCountry { get; set; }
}

class ErrorMessages
{
    public static string ValueIsNotValidCountryCode { get; } = "'{0}' is not a valid country code.";
}
```

### CurrencyCodeAttribute
Checks if the value is a valid 3-digit currency code. (i.e. 'USD') The currency code list can be customized. If Normalization is enabled, the value will be replaced with a capitalized string (interned). The validation succeeds if the value is null.
```csharp
class Model
{
    [CurrencyCode]
    public string CurrencyCode { get; set; }
}
```
```csharp
class Model
{
    [CurrencyCode(Normalized=true)]
    public string CurrencyCode { get; set; }  // after validated, the value will be replaced with a capitalized value. ('usd' becomes 'USD') 
}
```

### ElementRequiredAttribute
Checks if the value (array or collection) contains at least one element. The validation fails if the value is null.
```csharp
class Model
{
    [ElementRequired]
    public string[] Values { get; set; }
}
```
```csharp
class Model
{
    [ElementRequired]
    public List<string> Values { get; set; }
}
```

### FixedLengthAttribute
Checks if the value (string) has the specified length. The validation succeeds if the value is null.
```csharp
class Model
{
    [FixedLength(5)]
    public string ZipCode { get; set; }  // i.e. "90201"
}
```

### PastDateAttribute
Checks if the value (DateTime) is a past date and time. The validation succeeds if the value is null.
```csharp
class Model
{
    [PastDate]
    public DateTime StartDate { get; set; }
}
```

## ValidationExtensions
### \[object\].GetValidationResults()
```csharp
var model = new Model();
model.Value = null;

ICollection<string> errorMessages = model.GetValidationResults();  // [ "The field is required." ]

class Model
{
    [Required]  // to be run
    public string Value { get; set; }
}
```
```csharp
var model = new Model();
model.Value = "abc";

ICollection<string> errorMessages = model.GetValidationResults();  // [ "The field must match the regular expression '[A-Z]+'.", "Error" ]

class Model : IValidatableObject
{
    [RegularExpression("[A-Z]+")]  // to be run
    public string Value { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  // to be run
    {
        if (Value != null && Value.Length != 2)
        {
            yield return new ValidationResult("Error");
        }
    }
}
```
