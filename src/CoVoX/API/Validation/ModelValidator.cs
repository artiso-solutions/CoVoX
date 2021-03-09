using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Covox
{
    internal static class ModelValidator
    {
        public static List<ValidationResult> Validate(Configuration configuration)
        {
            var results = new List<ValidationResult>();

            if (configuration == null)
            {
                results.Add(new ValidationResult($"{nameof(configuration)} is invalid"));
                return results;
            }

            results = ValidateModel(configuration);

            if (configuration.AzureConfiguration is not null)
                results.AddRange(ValidateModel(configuration.AzureConfiguration));

            return results;
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var ctx = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, ctx, results, true);

            return results;
        }
    }
}
