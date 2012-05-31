using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Patterns;

namespace Utilities.Validation
{
    /// <summary>
    ///   This class implements a fluent-style validator.
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
        ///   Creates a new instance of this class.
        /// </summary>
        /// <param name="errors"> Validation results. </param>
        public FluentValidator(ICollection<ValidationResult> errors)
        {
            Errors = errors ?? new Collection<ValidationResult>();
            _initialErrorCount = Errors.Count;
        }

        /// <summary>
        ///   A combine error of all the validation errors
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
        ///   Get/set the validation results.
        /// </summary>
        public ICollection<ValidationResult> Errors { get; private set; }

        /// <summary>
        ///   True if there are errors.
        /// </summary>
        public bool HasErrors
        {
            get { return Errors.Count > _initialErrorCount; }
        }

        /// <summary>
        ///   Set the validation target object.
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
        ///   Set the validation expression.
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
        ///   Sets the property name and the target object.
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

        /// <summary>
        ///   Determines if a value contains a specified string.
        /// </summary>
        /// <param name="val"> Specified string. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator Contains(string val, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            if (((string)_target).IsNullOrEmpty())
                return errorMessage.IsNullOrEmpty() ? IsValid(false, "does not contain : " + val)
                    : IsValid(false, errorMessage);

            var valToCheck = (string)_target;
            return ErrorMessage.IsNullOrEmpty() ? IsValid(valToCheck.Contains(val), "must contain : " + val)
                : IsValid(valToCheck.Contains(val), errorMessage);
        }

        /// <summary>
        ///   Returns the current instance of this class.
        /// </summary>
        /// <returns> </returns>
        public FluentValidator End()
        {
            return this;
        }

        /// <summary>
        ///   Sets a check condition.
        /// </summary>
        /// <param name="isOkToCheckNext"> Validate condition. </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator If(bool isOkToCheckNext)
        {
            _checkCondition = isOkToCheckNext;
            return this;
        }

        /// <summary>
        ///   Determines if the value is in a list of values.
        /// </summary>
        /// <typeparam name="T"> Type of values in list. </typeparam>
        /// <param name="errorMessage">An optional error message </param>
        /// <param name="vals"> List of values. </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator In<T>(string errorMessage = "", params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals.IsNull() || vals.Length == 0)
                return this;

            var checkVal = _target.As<T>();
            var isValid = vals.Select(val => val.As<T>()).Contains(checkVal);

            return errorMessage.IsNullOrEmpty() ? IsValid(isValid, "is not a valid value") : IsValid(isValid, errorMessage);
        }

        /// <summary>
        ///   Validates against a specified value.
        /// </summary>
        /// <param name="val"> Value to validate against. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator Is(object val, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            if (_target.IsNull() && val.IsNull())
                return this;

            if (_target.IsNull() && val != null)
                return errorMessage.IsNullOrEmpty() ? IsValid(false, "must equal : " + val) : IsValid(false, errorMessage);

            return errorMessage.IsNullOrEmpty() ? IsValid(val.Equals(_target), "must equal : " + val) : IsValid(val.Equals(_target), errorMessage);
        }

        /// <summary>
        ///   Determines if a day is after a specified date.
        /// </summary>
        /// <param name="date"> Specified date. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsAfter(DateTime date, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var checkVal = (DateTime)_target;
            return errorMessage.IsNullOrEmpty()
                       ? IsValid(checkVal.Date.CompareTo(date.Date) > 0,
                                 "must be after date : " + date.ToShortDateString())
                       : IsValid(checkVal.Date.CompareTo(date.Date) > 0, errorMessage);
        }

        /// <summary>
        ///   Determines if a day is after today.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsAfterToday(string errorMessage = "")
        {
            if (!_checkCondition) return this;
            if (errorMessage.IsNullOrEmpty())
                IsAfter(DateTime.Today);
            else
                IsAfter(DateTime.Today, errorMessage);
            _checkCondition = false;
            return this;
        }

        /// <summary>
        ///   Determines if a day is before a specified date.
        /// </summary>
        /// <param name="date"> Specified date. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsBefore(DateTime date, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var checkVal = (DateTime)_target;
            return errorMessage.IsNullOrEmpty()
                       ? IsValid(checkVal.Date.CompareTo(date.Date) < 0,
                                 "must be before date : " + date.ToShortDateString())
                       : IsValid(checkVal.Date.CompareTo(date.Date) < 0, errorMessage);
        }

        /// <summary>
        ///   Determines if a day is before today.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsBeforeToday(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            if (errorMessage.IsNullOrEmpty())
                IsBefore(DateTime.Today);
            else
                IsBefore(DateTime.Today, errorMessage);

            _checkCondition = false;
            return this;
        }

        /// <summary>
        ///   Determines if a value is between a minimum and a maximum.
        /// </summary>
        /// <param name="min"> Minimum value. </param>
        /// <param name="max"> Maximum value. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsBetween(int min, int max, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var isNumeric = _target.IsNumeric();
            if (isNumeric)
            {
                var val = _target.As<decimal>();
                return errorMessage.IsNullOrEmpty()
                           ? IsValid(min <= val && val <= max, "must be between : " + min + ", " + max)
                           : IsValid(min <= val && val <= max, errorMessage);
            }
            // can only be string.
            var strVal = _target as string;
            if (min > 0 && string.IsNullOrEmpty(strVal))
                return errorMessage.IsNullOrEmpty() ? IsValid(false, "length must be between : " + min + ", " + max) :
                        IsValid(false, errorMessage);

            return errorMessage.IsNullOrEmpty() ? IsValid(strVal.IsNotNull() && (min <= strVal.Length && strVal.Length <= max),
                "length must be between : " + min + ", " + max) :
                IsValid(strVal.IsNotNull() && (min <= strVal.Length && strVal.Length <= max),
                errorMessage);
        }

        /// <summary>
        ///   Determines if a value is false.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsFalse(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var isBool = _target.Is<bool>();

            if(!isBool)
                return IsValid(false, "must be bool(true/false)");

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.As<bool>() == false, "must be false")
                       : IsValid(_target.As<bool>() == false, errorMessage);
        }

        /// <summary>
        ///   Determines if the validation is different than the specified value.
        /// </summary>
        /// <param name="val"> Specified value. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsNot(object val, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            if (_target.IsNull() && val.IsNull())
                return this;

            if (_target.IsNull() && val != null)
                return this;

            return errorMessage.IsNullOrEmpty() ? IsValid(!val.Equals(_target), "must not equal : " + val) :
                IsValid(!val.Equals(_target), errorMessage);
        }

        /// <summary>
        ///   Determines if the value is not null.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsNotNull(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty() ? IsValid(_target.IsNotNull(), "must be not null")
                : IsValid(_target.IsNotNull(), errorMessage);
        }

        /// <summary>
        ///   Determines if the value is null.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsNull(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.IsNull(), "must be null")
                       : IsValid(_target.IsNull(), errorMessage);
        }

        /// <summary>
        ///   Determines if a value is true.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsTrue(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var isBool = _target.Is<bool>();

            if (!isBool)
                IsValid(false, "must be bool(true/false)");

            return errorMessage.IsNullOrEmpty() ? IsValid(_target.As<bool>(), "must be true") : 
                IsValid(_target.As<bool>(),errorMessage);
        }

        /// <summary>
        ///   Determines if a value is a valid e-mail.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsValidEmail(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.ToString().IsEmail(), "must be a valid email.")
                       : IsValid(_target.ToString().IsEmail(), errorMessage);
        }

        /// <summary>
        ///   Determine if value is a valid MobileNumber
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsValidMobileNumber(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.ToString().IsMobileNumber(), "must be a valid zip.")
                       : IsValid(_target.ToString().IsMobileNumber(), errorMessage);
        }

        /// <summary>
        ///   Determines if a value is a valid zip code.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsValidPostcode(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.ToString().IsPostcode(), "must be a valid zip.")
                       : IsValid(_target.ToString().IsPostcode(), errorMessage);
        }

        /// <summary>
        ///   Determines if a value is a valid URL.
        /// </summary>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator IsValidUrl(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(_target.ToString().IsURL(), "must be a valid url.")
                       : IsValid(_target.ToString().IsURL(), errorMessage);
        }

        /// <summary>
        ///   Determines if a regular expression produces a match.
        /// </summary>
        /// <param name="regex"> Regular expression. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator Matches(string regex,string errorMessage = "")
        {
            if (!_checkCondition) return this;

            return errorMessage.IsNullOrEmpty()
                       ? IsValid(Regex.IsMatch((string) _target, regex), "does not match pattern : " + regex)
                       : IsValid(Regex.IsMatch((string) _target, regex), errorMessage);
        }

        /// <summary>
        ///   Determines if a value is equal or below a specified maximum.
        /// </summary>
        /// <param name="max"> Specified maximum. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator Max(int max, string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var isNumeric = _target.IsNumeric();
            if (!isNumeric) return IsValid(false, "must be a numeric value");

            var val = _target.As<decimal>();
            return errorMessage.IsNullOrEmpty()
                       ? IsValid(val <= max, "must have maximum value of : " + max)
                       : IsValid(val <= max, errorMessage);
        }

        /// <summary>
        ///   Determines if a value is equal or above a specified minimum.
        /// </summary>
        /// <param name="min"> Specified minimum. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator Min(int min,string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var isNumeric = _target.IsNumeric();
            if (!isNumeric) return IsValid(false, "must be numeric value");

            var val = _target.As<decimal>();
            return errorMessage.IsNullOrEmpty() ? IsValid(val >= min, "must have minimum value of : " + min):
                 IsValid(val >= min, errorMessage);
        }

        /// <summary>
        ///  Determines if property conforms with a custom action
        /// </summary>
        /// <param name="predicate">expression to execute on parameter</param>
        /// <param name="errorMessage">Error message to display if expression return false</param>
        /// <returns>The current instance of this class.</returns>
        public FluentValidator Must(Func<object, bool> predicate, string errorMessage)
        {
            if (predicate.IsNull()) throw new ArgumentNullException("predicate");
            var result = predicate(_target);
            return errorMessage.IsNullOrEmpty()
                       ? IsValid(result, "problem with Property")
                       : IsValid(result, errorMessage);
        }
       
        /// <summary>
        ///   Determines if a value does not contain a specified string.
        /// </summary>
        /// <param name="val"> Specified string. </param>
        /// <param name="errorMessage">An optional error message </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator NotContain(string val,string errorMessage = "")
        {
            if (!_checkCondition) return this;

            if (string.IsNullOrEmpty((string)_target))
                return this;

            var valToCheck = _target.As<string>();
            return errorMessage.IsNullOrEmpty() ? IsValid(!valToCheck.Contains(val), "should not contain : " + val):
                 IsValid(!valToCheck.Contains(val), errorMessage);
        }

        /// <summary>
        ///   Determines if the value is not in a list of values.
        /// </summary>
        /// <typeparam name="T"> Type of values in list. </typeparam>
        /// <param name="errorMessage"> </param>
        /// <param name="vals"> List of values. </param>
        /// <returns> The current instance of this class. </returns>
        public FluentValidator NotIn<T>(string errorMessage = "",params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals.IsNull() || vals.Length == 0)
                return this;

            var checkVal = _target.As<T>();
            var isValid = !vals.Select(val => val.As<T>()).Contains(checkVal);
            return errorMessage.IsNullOrEmpty() ? IsValid(isValid, "is not a valid value"):
                IsValid(isValid, errorMessage);
        }

        /// <summary>
        /// Check that property has been populated 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public FluentValidator Required(string errorMessage = "")
        {
            if (!_checkCondition) return this;

            var valid = _target.IsNot<string>() ? _target.IsNotNull() : _target.As<string>().IsNotNullOrEmpty();

            return errorMessage.IsNotNullOrEmpty() ? IsValid(valid, "is required") : IsValid(valid, errorMessage);
        }

        /// <summary>
        ///   Get the property name from the expression. e.g. GetPropertyName(Person)( p => p.FirstName);
        /// </summary>
        /// <param name="exp"> </param>
        /// <param name="propName"> </param>
        /// <returns> </returns>
        private static object GetPropertyNameAndValue(Expression<Func<object>> exp, ref string propName)
        {
            PropertyInfo propInfo = null;
            if (exp.Body is MemberExpression)
            {
                propInfo = ((MemberExpression)exp.Body).Member as PropertyInfo;
            }
            else if (exp.Body is UnaryExpression)
            {
                var op = ((UnaryExpression)exp.Body).Operand;
                propInfo = ((MemberExpression)op).Member as PropertyInfo;
            }

            var val = exp.Compile().DynamicInvoke();
            if (propInfo.IsNotNull()) propName = propInfo.Name.TitleCase();
            return val;
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
    }
}