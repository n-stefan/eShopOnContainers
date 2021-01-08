using System;
using System.ComponentModel.DataAnnotations;

namespace WebBlazor.Client.Services.ModelDTOs.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class LongitudeCoordinate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!double.TryParse(value.ToString(), out double coordinate) || (coordinate < -180 || coordinate > 180))
            {
                return new ValidationResult("Longitude must be between -180 and 180 degrees inclusive.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
