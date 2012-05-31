using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Utilities.Extensions;
using Utilities.Validation;

namespace Utilities._Samples
{
    /// <summary>
    ///   Example for the Validation namespace. This example can be dropped into a console application to see the Validation functionality.
    /// </summary>
    public class Example_Validation
    {
        private static void Main(string[] args)
        {
            var val = new FluentValidator();
            var user = new User();
            val.Validate(() => user.UserName).IsNotNull()
                .Validate(() => user.CreateDate).IsAfterToday()
                .Validate(() => user.Email).IsValidEmail()
                .Validate(() => user.MobilePhone).IsValidMobileNumber()
                .Validate(() => user.CreateDate).Must(BeThisYear, "Creation Date must be this year");

            PrintErrors(val.Errors);

            //Or call the IsValid method on the model its self
            //user.IsIsValid();
        }

        private static bool BeThisYear(object target)
        {
            if (target.IsNot<DateTime>())
                throw new ArgumentException("Cant validate a none datetime object");

            var date = target.As<DateTime>();

            return (date.Year == DateTime.Today.Year);
        }


        private static void PrintErrors(IEnumerable<ValidationResult> errors)
        {
            foreach (var combinedErrors in errors.Select(error => error.ErrorMessage))
            {
                Console.WriteLine("ERRORS: " + Environment.NewLine + combinedErrors);
            }
        }

        #region Nested type: User

        public class User : IValidatableObject 
        {
            public string UserName { get; set; }
            public DateTime CreateDate { get; set; }
            public string Email { get; set; }
            public string MobilePhone { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var val = new FluentValidator();
                val.Validate(() => UserName).IsNotNull()
                    .Validate(() => CreateDate).IsAfterToday()
                    .Validate(() => Email).IsValidEmail()
                    .Validate(() => MobilePhone).IsValidMobileNumber();

                return val.Errors;
            }
        }

        #endregion
    }
}