using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Covox.Attributes
{
    internal class MustHaveValidLanguages : ValidationAttribute
    {
        private static string GetErrorMessage(IEnumerable<string> notValidLanguages) =>
            $"These languages are invalid: {string.Join(",", notValidLanguages)}";
 
        private string GetErrorMessage(ValidationContext ctx) => string.Format(ErrorMessageString, ctx.MemberName);
            
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var validCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.Name).ToHashSet();
            
            if (value is string strValue && !string.IsNullOrWhiteSpace(strValue) && validCultures.Contains(strValue))
                return ValidationResult.Success;

            if (value is not IEnumerable<string> enumerable)
                return new ValidationResult(GetErrorMessage(validationContext));

            var invalidLanguages = enumerable
                .Where(item => !validCultures.Contains(item))
                .ToList();

            return invalidLanguages.Any()
                ? new ValidationResult(GetErrorMessage(invalidLanguages))
                : ValidationResult.Success;
        }
    }
}
