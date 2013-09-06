using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Utilities.Validation.ExtensionMethods
{
	public static class ValidationExtensions
	{
		/// <summary>
		///     Combine validation results errors into a single formatted
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

		/// <summary>
		///     Validate if the model is valid using its own data annotation
		/// </summary>
		/// <param name="validate">object to validate</param>
		/// <returns><c>true</c> if model is valid <c>false</c> if model is invalid</returns>
		public static bool TryValidate(this object validate)
		{
			return Validator.TryValidateObject(validate, new ValidationContext(validate, null, null), null);
		}

		/// <summary>
		///     Validate if the model is valid using its own data annotation
		/// </summary>
		/// <param name="validate">object to validate</param>
		/// <param name="validationResults">
		///     <c> outs a set of validation errors</c>
		/// </param>
		/// <returns><c>true</c> if model is valid <c>false</c> if model is invalid</returns>
		public static bool TryValidate(this object validate, out ICollection<ValidationResult> validationResults)
		{
			validationResults = new Collection<ValidationResult>();
			return Validator.TryValidateObject(validate, new ValidationContext(validate, null, null), validationResults);
		}

		/// <summary>
		///     Validate that the specified string contains only alpha characters. An exception is thrown if its not alpha
		/// </summary>
		/// <param name="input"> The string to validate. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static string ValidateAlpha(this string input, string paramName = null, string errorMessage = null)
		{
			if (Regex.IsMatch(input, RegexPattern.ALPHA))
			{
				if (errorMessage != null)
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was not an alpha value.", paramName ?? GetDefaultParameterName(input)));
			}

			return input;
		}

		/// <summary>
		///     Determines whether the specified string contains only alphanumeric characters An exception is thrown if its not
		///     alpha numeric
		/// </summary>
		/// <param name="input"> The string to validate. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static string ValidateAlphaNumeric(this string input, string paramName = null, string errorMessage = null)
		{
			if (Regex.IsMatch(input, RegexPattern.ALPHA_NUMERIC))
			{
				if (errorMessage.IsNotNullOrEmpty())
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was not an alpha numeric value.", paramName ?? GetDefaultParameterName(input)));
			}

			return input;
		}

		/// <summary>
		///     Validate whether the specified email address string is valid based on regular expression evaluation. An exception
		///     is thrown if its not numeric
		/// </summary>
		/// <param name="input"> The string to validate. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static string ValidateEmail(this string input, string paramName = null, string errorMessage = null)
		{
			if (Regex.IsMatch(input, RegexPattern.EMAIL))
			{
				if (errorMessage.IsNotNullOrEmpty())
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was not a valid email address.", paramName ?? GetDefaultParameterName(input)));
			}

			return input;
		}

		/// <summary>
		///     Validate that the supplied object is one of a items of objects.
		/// </summary>
		/// <param name="instance"> Object to look for. </param>
		/// <param name="possibles"> List with possible values for object. </param>
		/// <param name="errorMessage"> Message of exception to throw. </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static T ValidateIsOneOfSupplied<T>(this T instance, List<T> possibles, string errorMessage)
		{
			if (Enumerable.Contains(possibles, instance))
			{
				return instance;
			}
			throw new ValidationException(errorMessage);
		}

		/// <summary>
		///     Validate that the condition is true and return error errorMessage provided.
		/// </summary>
		/// <param name="condition"> Condition to check. </param>
		/// <param name="message"> Error to use when throwing an <see cref="ArgumentException" /> if the condition is false. </param>
		public static void ValidateIsTrue(bool condition, string message = null)
		{
			if (!condition)
			{
				throw new ValidationException(message);
			}
		}

		/// <summary>
		///     Validates that the specified source instance is not null.
		/// </summary>
		/// <param name="instance"> Object to check. </param>
		/// <param name="paramName">name of parameter </param>
		/// <param name="errorMessage"> Description of the object being validated that will be added to error errorMessage. </param>
		/// <returns> Object supplied </returns>
		public static T ValidateNotNull<T>(this T instance, string paramName = null, string errorMessage = null)
		{
			if (instance == null)
			{
				if (errorMessage != null)
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was null and failed validation.", paramName ?? GetDefaultParameterName(instance)));
			}
			return instance;
		}

		/// <summary>
		///     Validates that the source text is not null or empty using the specified errorMessage in the error errorMessage
		/// </summary>
		/// <param name="input"> The string to validate. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static string ValidateNotNullOrEmpty(this string input, string paramName = null, string errorMessage = null)
		{
			if (string.IsNullOrEmpty(input))
			{
				if (errorMessage != null)
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was not null or not empty.", paramName ?? GetDefaultParameterName(input)));
			}
			return input;
		}

		/// <summary>
		///     Validates that the specified instance is not zero
		/// </summary>
		/// <param name="instance"> Object to check. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="instance"></paramref>  is not valid </exception>
		public static T ValidateNotZero<T>(this T instance, string paramName = null, string errorMessage = null)
		{
			instance.ValidateNotNull(errorMessage);
			if (instance.ToString().Equals("0"))
			{
				if (errorMessage != null)
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was zero and failed validation.", paramName ?? GetDefaultParameterName(instance)));
			}
			return instance;
		}

		/// <summary>
		///     Validates the specified string contains only numeric characters An exception is thrown if its not numeric
		/// </summary>
		/// <param name="input"> The string to validate. </param>
		/// <param name="paramName">An optional name of the input parameter (This will default to the name of the input)</param>
		/// <param name="errorMessage">An optional custom error message (This will override any custom paramName provided </param>
		/// <exception cref="ValidationException">Thrown when <paramref name="input"></paramref>  is not valid </exception>
		public static string ValidateNumeric(this string input, string paramName = null, string errorMessage = null)
		{
			if (Regex.IsMatch(input, RegexPattern.NUMERIC))
			{
				if (errorMessage != null)
					throw new ValidationException(errorMessage);
				throw new ValidationException(string.Format("The {0} was not a numeric value.", paramName ?? GetDefaultParameterName(input)));
			}

			return input;
		}

		private static string GetDefaultParameterName<T>(T input)
		{
			return TypeHelper.GetName(new {input});
		}
	}
}