using System;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Validation.ValidationAttributes
{
    /// <summary>
    /// A validation attribute for year 
    /// <remarks> To get client side validation you need to register on app statup
    /// DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(UrlAttribute), typeof(RegularExpressionAttributeAdapter));</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class YearAttibute :RegularExpressionAttribute
    {
        public YearAttibute() : base(RegexPattern.YEAR)
        {

        }
    }
}