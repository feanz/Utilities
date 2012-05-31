using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Validation.ExtensionMethods
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validate if the model is valide using its own data annotation
        /// </summary>
        /// <param name="validate">object to validate</param>
        /// <returns><c>true</c> if model is valide <c>false</c> if model is invalide</returns>
        public static bool TryValidate(this object validate)
        {
            return Validator.TryValidateObject(validate, new ValidationContext(validate, null, null), null);
        }

        /// <summary>
        /// Validate if the model is valide using its own data annotation
        /// </summary>
        /// <param name="validate">object to validate</param>
        /// <param name="validationResults"><c> outs a set of validation errors</c> </param>
        /// <returns><c>true</c> if model is valide <c>false</c> if model is invalide</returns>
        public static bool TryValidate(this object validate, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new Collection<ValidationResult>();
            return Validator.TryValidateObject(validate, new ValidationContext(validate, null, null), validationResults);
        }

        /// <summary>
        /// Combine validation results erros into a single formatted 
        /// </summary>
        /// <param name="validationResults">A set of Validation results to combine </param>
        /// <returns>The combined error message</returns>
        public static string CombinedErrorMessage(this ICollection<ValidationResult> validationResults)
        {
            var combine = "Error Validating";

            foreach (var v in validationResults)
            {
                combine += v.MemberNames;
                combine += ":  ";
                combine += v.ErrorMessage;
                combine += Environment.NewLine;
            }

            return combine;
        }
    }
}