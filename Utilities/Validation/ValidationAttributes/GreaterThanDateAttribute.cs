using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Utilities.Validation.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class GreaterThanDateAttribute : ValidationAttribute, IClientValidatable
    {
        public GreaterThanDateAttribute(string otherPropertyName, string otherPropertyDisplayName)
            : base("{0} must be greater than {1}")
        {
            OtherPropertyName = otherPropertyName;
            OtherPropertyDisplayName = otherPropertyDisplayName;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            var otherDate = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            var thisDate = (DateTime)value;

            if (thisDate <= otherDate)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }

            return null;
        }
        
        public string OtherPropertyName { get; private set; }

        public string OtherPropertyDisplayName { get; private set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "greater";
            rule.ValidationParameters.Add("other", OtherPropertyName);
            yield return rule;
        }

        //Client side validation code for jqurey validate 
        //        jQuery.validator.addMethod("greater", function (value, element, param) {
        //    return Date.parse(value) > Date.parse($(param).val());
        //});

        //jQuery.validator.unobtrusive.adapters.add("greater", ["other"], function (options) {
        //    options.rules["greater"] = "#" + options.params.other;
        //    options.messages["greater"] = options.errorMessage;
        //}); 
    }
}