using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class FluentValidationTool
    {
        public static void Validate(IValidator _validator, object entity, out IList<ValidationFailure> errors)
        {
            var result = _validator.Validate(entity);
            errors = null;
            if (!result.IsValid)
            {
                errors = result.Errors;
            }
        }
    }
}
