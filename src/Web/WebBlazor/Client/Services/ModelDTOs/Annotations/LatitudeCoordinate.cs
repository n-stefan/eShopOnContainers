using System;
using System.ComponentModel.DataAnnotations;

namespace WebBlazor.Client.Services.ModelDTOs.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class LatitudeCoordinate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!double.TryParse(value.ToString(), out var coordinate) || (coordinate < -90 || coordinate > 90))
            {
                return new("Latitude must be between -90 and 90 degrees inclusive.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
