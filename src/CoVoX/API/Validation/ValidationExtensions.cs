using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Covox
{
    internal static class ValidationExtensions
    {
        public static Exception AsException(this List<ValidationResult> errors)
        {
            var exceptions = errors.Select(error => new Exception(error.ErrorMessage));
            return new AggregateException(exceptions);
        }
    }
}
