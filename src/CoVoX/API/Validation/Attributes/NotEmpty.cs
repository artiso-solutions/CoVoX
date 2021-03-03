using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Covox.Validation
{
    internal class NotEmpty : ValidationAttribute
    {
        private string GetErrorMessage(ValidationContext ctx) => string.Format(ErrorMessageString, ctx.MemberName);
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
                return ValidationResult.Success;
            
            if (value is IEnumerable enumerable && enumerable.Cast<object>().Any())
                return ValidationResult.Success;
            
            var result = new ValidationResult(GetErrorMessage(validationContext));

            return result;
        }
    }
}
