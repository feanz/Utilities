using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Utilities.Patterns;

namespace Utilities.Validation
{
	/// <summary>
	///     This class implements a fluent-style validator.
	/// </summary>
	public class FluentValidator : IFluentInterface
	{
		private readonly int _initialErrorCount;
		private bool _checkCondition;
		private string _propertyName;
		private object _target;

		public FluentValidator()
		{
			Errors = new Collection<ValidationResult>();
			_initialErrorCount = 0;
		}

		/// <summary>
		///     Creates a new instance of this class.
		/// </summary>
		/// <param name="errors"> Validation results. </param>
		public FluentValidator(ICollection<ValidationResult> errors)
		{
			Errors = errors ?? new Collection<ValidationResult>();
			_initialErrorCount = Errors.Count;
		}

		/// <summary>
		///     A combine error of all the validation errors
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				var result = string.Empty;
				foreach (var error in Errors.Select(x => x.ErrorMessage))
				{
					result += error;
					result += Environment.NewLine;
				}
				return result;
			}
		}

		/// <summary>
		///     Get/set the validation results.
		/// </summary>
		public ICollection<ValidationResult> Errors { get; private set; }

		/// <summary>
		///     True if there are errors.
		/// </summary>
		public bool HasErrors
		{
			get { return Errors.Count > _initialErrorCount; }
		}

		/// <summary>
		///     Determines if a value contains a specified string.
		/// </summary>
		/// <param name="val"> Specified string. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Contains(string val, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			if (string.IsNullOrEmpty((string) _target))
				return string.IsNullOrEmpty(errorMessage) ? IsValid(false, "does not contain : " + val)
					: IsValid(false, errorMessage);

			var valToCheck = (string) _target;
			return string.IsNullOrEmpty(ErrorMessage) ? IsValid(valToCheck.Contains(val), "must contain : " + val)
				: IsValid(valToCheck.Contains(val), errorMessage);
		}

		/// <summary>
		///     Returns the current instance of this class.
		/// </summary>
		/// <returns> </returns>
		public FluentValidator End()
		{
			return this;
		}

		/// <summary>
		///     Sets a check condition.
		/// </summary>
		/// <param name="isOkToCheckNext"> Validate condition. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator If(bool isOkToCheckNext)
		{
			_checkCondition = isOkToCheckNext;
			return this;
		}

		/// <summary>
		///     Determines if the value is in a list of values.
		/// </summary>
		/// <typeparam name="T"> Type of values in list. </typeparam>
		/// <param name="errorMessage">An optional error message </param>
		/// <param name="vals"> List of values. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator In<T>(string errorMessage = "", params object[] vals)
		{
			if (!_checkCondition) return this;

			if (vals == null || vals.Length == 0)
				return this;

			bool isValid;
			try
			{
				var checkVal = (T) _target;
				isValid = vals.Select(val => (T) val).Contains(checkVal);
			}
			catch (InvalidCastException)
			{
				isValid = false;
			}

			return string.IsNullOrEmpty(errorMessage) ? IsValid(isValid, "is not a valid value") : IsValid(isValid, errorMessage);
		}

		/// <summary>
		///     Validates against a specified value.
		/// </summary>
		/// <param name="val"> Value to validate against. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Is(object val, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			if (_target == null || val == null)
				return this;

			if (_target == null)
				return string.IsNullOrEmpty(errorMessage) ? IsValid(false, "must equal : " + val) : IsValid(false, errorMessage);

			return string.IsNullOrEmpty(errorMessage) ? IsValid(val.Equals(_target), "must equal : " + val) : IsValid(val.Equals(_target), errorMessage);
		}

		/// <summary>
		///     Determines if a day is after a specified date.
		/// </summary>
		/// <param name="date"> Specified date. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsAfter(DateTime date, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var checkVal = (DateTime) _target;
			return string.IsNullOrEmpty(errorMessage)
				? IsValid(checkVal.Date.CompareTo(date.Date) > 0,
					"must be after date : " + date.ToShortDateString())
				: IsValid(checkVal.Date.CompareTo(date.Date) > 0, errorMessage);
		}

		/// <summary>
		///     Determines if a day is after today.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsAfterToday(string errorMessage = "")
		{
			if (!_checkCondition) return this;
			if (string.IsNullOrEmpty(errorMessage))
				IsAfter(DateTime.Today);
			else
				IsAfter(DateTime.Today, errorMessage);
			_checkCondition = false;
			return this;
		}

		/// <summary>
		///     Determines if a day is before a specified date.
		/// </summary>
		/// <param name="date"> Specified date. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsBefore(DateTime date, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var checkVal = (DateTime) _target;
			return string.IsNullOrEmpty(errorMessage)
				? IsValid(checkVal.Date.CompareTo(date.Date) < 0,
					"must be before date : " + date.ToShortDateString())
				: IsValid(checkVal.Date.CompareTo(date.Date) < 0, errorMessage);
		}

		/// <summary>
		///     Determines if a day is before today.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsBeforeToday(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			if (string.IsNullOrEmpty(errorMessage))
				IsBefore(DateTime.Today);
			else
				IsBefore(DateTime.Today, errorMessage);

			_checkCondition = false;
			return this;
		}

		/// <summary>
		///     Determines if a value is between a minimum and a maximum.
		/// </summary>
		/// <param name="min"> Minimum value. </param>
		/// <param name="max"> Maximum value. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsBetween(int min, int max, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var isNumeric = TypeHelper.IsNumeric(_target.GetType());
			if (isNumeric)
			{
				var val = (decimal) _target;
				return string.IsNullOrEmpty(errorMessage)
					? IsValid(min <= val && val <= max, "must be between : " + min + ", " + max)
					: IsValid(min <= val && val <= max, errorMessage);
			}
			// can only be string.
			var strVal = _target as string;
			if (min > 0 && !string.IsNullOrEmpty(strVal))
				return string.IsNullOrEmpty(errorMessage) ? IsValid(false, "length must be between : " + min + ", " + max) :
					IsValid(false, errorMessage);

			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(strVal != null && (min <= strVal.Length && strVal.Length <= max), "length must be between : " + min + ", " + max) :
				IsValid(strVal != null && (min <= strVal.Length && strVal.Length <= max), errorMessage);
		}

		/// <summary>
		///     Determines if a value is false.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsFalse(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var isBool = _target is bool;

			if (!isBool)
				return IsValid(false, "must be boolean (true/false)");

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(((bool) _target) == false, "must be false")
				: IsValid(((bool) _target) == false, errorMessage);
		}

		/// <summary>
		///     Determines if the validation is different than the specified value.
		/// </summary>
		/// <param name="val"> Specified value. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsNot(object val, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			if (_target == null || val == null)
				return this;

			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(!val.Equals(_target), "must not equal : " + val) :
				IsValid(!val.Equals(_target), errorMessage);
		}

		/// <summary>
		///     Determines if the value is not null.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsNotNull(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage) ? IsValid(_target != null, "must be not null")
				: IsValid(_target != null, errorMessage);
		}

		/// <summary>
		///     Determines if the value is null.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsNull(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(_target == null, "must be null")
				: IsValid(_target == null, errorMessage);
		}

		/// <summary>
		///     Determines if a value is true.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsTrue(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var isBool = _target is bool;

			if (!isBool)
				IsValid(false, "must be bool(true/false)");

			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(((bool) _target), "must be true") :
				IsValid(((bool) _target), errorMessage);
		}

		/// <summary>
		///     Determines if a value is a valid e-mail.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsValidEmail(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(IsEmail(_target.ToString()), "must be a valid email.")
				: IsValid(IsEmail(_target.ToString()), errorMessage);
		}

		/// <summary>
		///     Determine if value is a valid MobileNumber
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsValidMobileNumber(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(IsMobileNumber(_target.ToString()), "must be a valid zip.")
				: IsValid(IsMobileNumber(_target.ToString()), errorMessage);
		}

		/// <summary>
		///     Determines if a value is a valid zip code.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsValidPostcode(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(IsPostcode(_target.ToString()), "must be a valid zip.")
				: IsValid(IsPostcode(_target.ToString()), errorMessage);
		}

		/// <summary>
		///     Determines if a value is a valid URL.
		/// </summary>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator IsValidUrl(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(IsURL(_target.ToString()), "must be a valid url.")
				: IsValid(IsURL(_target.ToString()), errorMessage);
		}

		/// <summary>
		///     Determines if a regular expression produces a match.
		/// </summary>
		/// <param name="regex"> Regular expression. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Matches(string regex, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			return string.IsNullOrEmpty(errorMessage)
				? IsValid(Regex.IsMatch((string) _target, regex), "does not match pattern : " + regex)
				: IsValid(Regex.IsMatch((string) _target, regex), errorMessage);
		}

		/// <summary>
		///     Determines if a value is equal or below a specified maximum.
		/// </summary>
		/// <param name="max"> Specified maximum. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Max(int max, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var isNumeric = TypeHelper.IsNumeric(_target.GetType());
			if (!isNumeric) return IsValid(false, "must be a numeric value");

			var val = (decimal) _target;
			return string.IsNullOrEmpty(errorMessage)
				? IsValid(val <= max, "must have maximum value of : " + max)
				: IsValid(val <= max, errorMessage);
		}

		/// <summary>
		///     Determines if a value is equal or above a specified minimum.
		/// </summary>
		/// <param name="min"> Specified minimum. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Min(int min, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var isNumeric = TypeHelper.IsNumeric(_target.GetType());

			if (!isNumeric) return IsValid(false, "must be numeric value");

			var val = (decimal) _target;
			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(val >= min, "must have minimum value of : " + min) :
				IsValid(val >= min, errorMessage);
		}

		/// <summary>
		///     Determines if property conforms with a custom action
		/// </summary>
		/// <param name="predicate">expression to execute on parameter</param>
		/// <param name="errorMessage">Error message to display if expression return false</param>
		/// <returns>The current instance of this class.</returns>
		public FluentValidator Must(Func<object, bool> predicate, string errorMessage)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			var result = predicate(_target);
			return string.IsNullOrEmpty(errorMessage)
				? IsValid(result, "problem with Property")
				: IsValid(result, errorMessage);
		}

		/// <summary>
		///     Determines if a value does not contain a specified string.
		/// </summary>
		/// <param name="val"> Specified string. </param>
		/// <param name="errorMessage">An optional error message </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator NotContain(string val, string errorMessage = "")
		{
			if (!_checkCondition) return this;

			if (string.IsNullOrEmpty((string) _target))
				return this;

			var valToCheck = (string) _target;
			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(!valToCheck.Contains(val), "should not contain : " + val) :
				IsValid(!valToCheck.Contains(val), errorMessage);
		}

		/// <summary>
		///     Determines if the value is not in a list of values.
		/// </summary>
		/// <typeparam name="T"> Type of values in list. </typeparam>
		/// <param name="errorMessage"> </param>
		/// <param name="vals"> List of values. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator NotIn<T>(string errorMessage = "", params object[] vals)
		{
			if (!_checkCondition) return this;

			if (vals == null || vals.Length == 0)
				return this;

			bool isValid;

			try
			{
				var checkVal = (T) _target;
				isValid = !vals.Select(val => (T) val).Contains(checkVal);
			}
			catch (InvalidCastException)
			{
				isValid = false;
			}

			return string.IsNullOrEmpty(errorMessage) ?
				IsValid(isValid, "is not a valid value") :
				IsValid(isValid, errorMessage);
		}

		/// <summary>
		///     Check that property has been populated
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public FluentValidator Required(string errorMessage = "")
		{
			if (!_checkCondition) return this;

			var valid = !(_target is string) ? _target != null : string.IsNullOrEmpty((string) _target);

			return string.IsNullOrEmpty(errorMessage) ? IsValid(valid, "is required") : IsValid(valid, errorMessage);
		}

		/// <summary>
		///     Set the validation target object.
		/// </summary>
		/// <param name="target"> The target object. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Validate(object target)
		{
			// Reset the check condition flag.
			_checkCondition = true;
			_propertyName = string.Empty;
			_target = target;
			return this;
		}

		/// <summary>
		///     Set the validation expression.
		/// </summary>
		/// <param name="exp"> Validation expression. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Validate(Expression<Func<object>> exp)
		{
			_target = GetPropertyNameAndValue(exp, ref _propertyName);
			_checkCondition = true;
			return this;
		}

		/// <summary>
		///     Sets the property name and the target object.
		/// </summary>
		/// <param name="propName"> Property name. </param>
		/// <param name="target"> Target object. </param>
		/// <returns> The current instance of this class. </returns>
		public FluentValidator Validate(string propName, object target)
		{
			// Reset the check condition flag.            
			_checkCondition = true;
			_propertyName = propName;
			_target = target;
			return this;
		}

		private FluentValidator IsValid(bool isValid, string error)
		{
			if (!isValid)
			{
				var prefix = string.IsNullOrEmpty(_propertyName) ? "Property " : _propertyName + " ";
				Errors.Add(new ValidationResult(prefix + error));
			}
			return this;
		}

		protected static bool IsEmail(string emailaddress)
		{
			return Regex.IsMatch(emailaddress, RegexPattern.EMAIL);
		}

		protected static bool IsMobileNumber(string mobileNumber)
		{
			return Regex.IsMatch(mobileNumber, RegexPattern.MOBILENUMBER);
		}

		protected static bool IsPostcode(string postcode)
		{
			return Regex.IsMatch(postcode, RegexPattern.POSTCODE);
		}

		protected static bool IsURL(string url)
		{
			return Regex.IsMatch(url, RegexPattern.URL);
		}

		/// <summary>
		///     Get the property name from the expression. e.g. GetPropertyName(Person)( p => p.FirstName);
		/// </summary>
		/// <param name="exp"> </param>
		/// <param name="propName"> </param>
		/// <returns> </returns>
		private static object GetPropertyNameAndValue(Expression<Func<object>> exp, ref string propName)
		{
			PropertyInfo propInfo = null;
			var expression = exp.Body as MemberExpression;
			if (expression != null)
			{
				propInfo = expression.Member as PropertyInfo;
			}
			else
			{
				var body = exp.Body as UnaryExpression;
				if (body != null)
				{
					var op = body.Operand;
					propInfo = ((MemberExpression) op).Member as PropertyInfo;
				}
			}

			var val = exp.Compile().DynamicInvoke();
			if (propInfo != null) propName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propInfo.Name);
			return val;
		}
	}
}