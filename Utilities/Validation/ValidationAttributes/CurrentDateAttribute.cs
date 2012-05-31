using System;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Validation.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
            : base("{0} is not current (between {1:d} and {2:d}")
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(System.Globalization.CultureInfo.CurrentCulture, ErrorMessageString, name, MinDate, MaxDate);
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var minDate = MinDate;
            var maxDate = MaxDate;

            if (date < minDate || date > maxDate)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }

            return null;
        }

        private readonly DateTime MinDate = DateTime.Now.AddMonths(-6);

        private readonly DateTime MaxDate = DateTime.Now.AddDays(7);
    }
}