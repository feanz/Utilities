using System;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Validation.ValidationAttributes
{
    /// <summary>
    /// A validation attribute for email address
    /// <remarks> To get client side validation you need to register on app statup
    /// DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAttribute), typeof(RegularExpressionAttributeAdapter));</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(RegexPattern.EMAIL)
        { }
    }
}