using System;
using System.ComponentModel.DataAnnotations;

namespace WebBlazor.Client.Services.ModelDTOs.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class CardExpirationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var valueSplit = value.ToString().Split('/');
            var monthString = valueSplit[0];
            var yearString = $"20{valueSplit[1]}";

            if (int.TryParse(monthString, out var month) && int.TryParse(yearString, out var year))
            {
                var date = new DateTime(year, month, 1);
                return date > DateTime.UtcNow;
            }
            else
            {
                return false;
            }
        }
    }
}
