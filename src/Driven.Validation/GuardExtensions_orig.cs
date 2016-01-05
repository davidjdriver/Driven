using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Driven.Validation
{
    public static class GuardExtensions
    {
        private static Regex regexEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        public static Guard ExtAddViolation(this Guard guard, GuardViolation guardViolation)
        {
            return (guard ?? new Guard()).AddViolation(guardViolation);
        }

        // used if you want to raise the existing exceptions
        public static Guard Check(this Guard guard)
        {
            if (guard == null)
            {
                return guard;
            }

            // these messages have to come from somewhere, not sure where at this point
            if (guard.Exceptions.Take(2).Count() == 1)
            {
                throw new GuardException("Message", guard.Exceptions.First());
            }

            throw new GuardMultiException("Message", guard.Exceptions);
        }

        // used if you just want fluent validation
        public static Guard CheckWithoutRaise(this Guard guard)
        {
            return guard ?? new Guard();
        }

        public static Guard Null<T>(this Guard guard, T paramValue, string paramName)
             where T : class
        {
            if (paramValue == null || DBNull.Value.Equals(paramValue))
            {
                return guard.ExtAddException(new ArgumentNullException(paramName));
            }

            return guard;
        }

        public static Guard Default<T>(this Guard guard, T paramValue, string paramName)
            where T : struct
        {
            if (paramValue.Equals(default(T)))
            {
                return guard.ExtAddException(new ArgumentException("Structure has default values", paramName));
            }

            return guard;
        }

        public static Guard NullNullable<T>(this Guard guard, Nullable<T> paramValue, string paramName)
             where T : struct, IComparable<T>
        {
            if (!paramValue.HasValue)
            {
                return guard.ExtAddException(new ArgumentNullException(paramName));
            }

            return guard;
        }

        public static Guard Negative<T>(this Guard guard, T paramValue, string paramName)
            where T : struct, IConvertible, IComparable
        {
            if (paramValue.CompareTo(0) < 0)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(paramName, "must be positive, but was " + paramValue.ToString()));
            }

            return guard;
        }

        public static Guard Positive<T>(this Guard guard, T paramValue, string paramName)
            where T : struct, IConvertible, IComparable
        {
            if (paramValue.CompareTo(0) > 0)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(paramName, "must be positive, but was " + paramValue.ToString()));
            }

            return guard;
        }

        public static Guard GreaterThan<T>(this Guard guard, T compareValue, T paramValue, string paramName)
            where T : struct, IConvertible, IComparable
        {
            if (paramValue.CompareTo(compareValue) > 0)
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Value must not be greater than {0}, value {1}", compareValue, paramValue), paramName));
            }

            return guard;
        }

        public static Guard LessThan<T>(this Guard guard, T compareValue, T paramValue, string paramName)
            where T : struct, IConvertible, IComparable
        {
            if (paramValue.CompareTo(compareValue) < 0)
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Value must not be less than {0}, value {1}", compareValue, paramValue), paramName));
            }

            return guard;
        }

        public static Guard Between<T>(this Guard guard, T startValue, T endValue, T paramValue, string paramName)
            where T : struct, IConvertible, IComparable
        {
            if (paramValue.CompareTo(startValue) > 0 || paramValue.CompareTo(endValue) < 0)
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Value must be between {0} and {1}, value {2}", startValue, endValue, paramValue), paramName));
            }

            return guard;
        }

        public static Guard NotBetween<T>(this Guard guard, T startValue, T endValue, T paramValue, string paramName)
            where T : IComparable
        {
            if (!(paramValue.CompareTo(startValue) >= 0 && paramValue.CompareTo(endValue) <= 0))
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Value not must be between {0} and {1}, value {2}", startValue, endValue, paramValue), paramName));
            }

            return guard;
        }

        public static Guard StringNullOrEmpty(this Guard guard, string paramValue, string paramName)
        {
            if (StringIsNull(paramValue))
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(paramName, "must not be null and must not be blank"));
            }

            return guard;
        }

        public static Guard StringLengthBetween(this Guard guard, int minLength, int maxLengh, string paramValue, string paramName)
        {
            var valueLen = paramValue.Length;
            if (((valueLen >= minLength) && (valueLen <= maxLengh)))
            {
                return guard.ExtAddException(new ArgumentException(string.Format("string length {1} not between the required range of {1} and {2}", valueLen, minLength, maxLengh), paramName));
            }

            return guard;
        }

        public static Guard StringLengthNotBetween(this Guard guard, int minLength, int maxLengh, string paramValue, string paramName)
        {
            var valueLen = paramValue.Length;
            if (!((valueLen >= minLength) && (valueLen <= maxLengh)))
            {
                return guard.ExtAddException(new ArgumentException(string.Format("string length {1} not between the required range of {1} and {2}", valueLen, minLength, maxLengh), paramName));
            }

            return guard;
        }

        public static Guard StringNotNumeric(this Guard guard, string paramValue, string paramName)
        {
            double outVal;
            if (!(double.TryParse(paramValue, out outVal)))
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Parameter {0} expected to be a numeric string, received {1}", paramName, paramValue)));
            }

            return guard;
        }

        public static Guard StringNumeric(this Guard guard, string paramValue, string paramName)
        {
            double outVal;
            if (double.TryParse(paramValue, out outVal))
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Parameter {0} expected to be a non-numeric string, received {1}", paramName, paramValue)));
            }

            return guard;
        }

        public static Guard NotEmail(this Guard guard, string paramValue, string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramValue) || !regexEmail.IsMatch(paramValue))
            {
                return guard.ExtAddException(new ArgumentException("must be a valid email address", paramName));
            }

            return guard;
        }

        public static Guard NotUrl(this Guard guard, string paramValue, string paramName)
        {
            Uri uri;
            bool validUri = false;
            if (Uri.TryCreate(paramValue, UriKind.Absolute, out uri))
            {
                validUri = (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            }

            if (!validUri)
            {
                return guard.ExtAddException(new ArgumentException(string.Format("Expected a valid HTTP or HTTPS URL on the parameter {0}, received {1}", paramName, paramValue)));
            }

            return guard;
        }

        public static Guard MatchesRegex(this Guard guard, string pattern, string paramValue, string paramName)
        {
            Regex exp = new Regex(pattern);
            if (string.IsNullOrWhiteSpace(paramValue) || exp.IsMatch(paramValue))
            {
                return guard.ExtAddException(new ArgumentException("must match regex: " + pattern, paramName));
            }

            return guard;
        }

        public static Guard NotMatchesRegex(this Guard guard, string pattern, string paramValue, string paramName)
        {
            Regex exp = new Regex(pattern);
            if (string.IsNullOrWhiteSpace(paramValue) || !exp.IsMatch(paramValue))
            {
                return guard.ExtAddException(new ArgumentException("must not match regex: " + pattern, paramName));
            }

            return guard;
        }

        //.ArgumentOutOfRange((collection.Count == 1), "arg1", "collection must not have one item")
        public static Guard ArgumentOutOfRange(this Guard guard, bool condition, string paramName, string message)
        {
            if (condition)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(paramName, message));
            }

            return guard;
        }

        // like above but without the parameter name parameter
        public static Guard InvalidOperation(this Guard guard, bool condition, string message)
        {
            if (condition)
            {
                return guard.ExtAddException(new InvalidOperationException(message));
            }

            return guard;
        }

        public static Guard StringIsNotEnum(this Guard guard, Type enumType, string paramValue, string paramName)
        {
            // check we are comparing an enum
            if (!enumType.IsEnum)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Guard StringIsNotEnum enumType parameter requires an enum but received {0}", enumType)));
            }

            // find out if the string is in the enum
            if (!Enum.IsDefined(enumType, paramValue))
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Enum {0} does not contain {1}", enumType, paramValue)));
            }

            return guard;
        }

        public static Guard IntIsNotEnum(this Guard guard, Type enumType, int paramValue, string paramName)
        {

            // check we are comparing an enum
            if (!enumType.IsEnum)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Guard StringIsNotEnum enumType parameter requires an enum but received {0}", enumType)));
            }

            // find out if the string is in the enum
            if (!Enum.IsDefined(enumType, paramValue))
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Enum {0} does not contain {1}", enumType, paramValue)));
            }

            return guard;
        }

        public static Guard InvaidCreditCardNumber(this Guard guard, string paramValue, string paramName)
        {
            if (!IsValidCreditCard(paramValue))
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Expected valid Credit Card Number but received {0}", paramValue)));
            }

            return guard;
        }

        public static Guard ContainsCharacters(this Guard guard, char[] testCharacters, string paramValue, string paramName)
        {
            if (paramValue.IndexOfAny(testCharacters) != -1)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Expected {0} to not contain the following characters {1}, received {1} which does contain one of these characters", paramName, testCharacters, paramValue)));
            }

            return guard;
        }

        public static Guard ContainsCharacters(this Guard guard, string testCharacters, string paramValue, string paramName)
        {
            if (paramValue.IndexOfAny(testCharacters.ToCharArray()) != -1)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Expected {0} to not contain the following characters {1}, received {1} which does contain one of these characters", paramName, testCharacters, paramValue)));
            }

            return guard;
        }

        public static Guard DoesNotContainCharacters(this Guard guard, char[] testCharacters, string paramValue, string paramName)
        {
            if (paramValue.IndexOfAny(testCharacters) == -1)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Expected {0} to contain one of the following characters {1}, received {1} which does contain one of these characters", paramName, testCharacters, paramValue)));
            }

            return guard;
        }

        public static Guard DoesNotContainCharacters(this Guard guard, string testCharacters, string paramValue, string paramName)
        {
            if (paramValue.IndexOfAny(testCharacters.ToCharArray()) == -1)
            {
                return guard.ExtAddException(new ArgumentOutOfRangeException(string.Format("Expected {0} to contain one of the following characters {1}, received {1} which does contain one of these characters", paramName, testCharacters, paramValue)));
            }

            return guard;
        }

        public static Guard WeakPassword(this Guard guard, string paramValue, string paramName)
        {
            throw new NotImplementedException();
        }

        private static bool StringIsNull(string paramValue)
        {
            return (String.IsNullOrWhiteSpace(paramValue) || DBNull.Value.Equals(paramValue));
        }

        //http://blogfornet.com/2013/07/validate-credit-card-using-luhn-algorithm/
        private static bool IsValidCreditCard(string cardNumber)
        {
            const string allowed = "0123456789";
            int i, multiplier, digit, sum, total = 0;
            string number;

            if (StringIsNull(cardNumber))
            {
                return false;
            }

            StringBuilder cleanNumber = new StringBuilder();
            for (i = 0; i < cardNumber.Length; i++)
            {
                if (allowed.IndexOf(cardNumber.Substring(i, 1)) >= 0)
                    cleanNumber.Append(cardNumber.Substring(i, 1));
            }

            if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
            {
                return false;
            }

            for (i = cleanNumber.Length + 1; i <= 16; i++)
            {
                cleanNumber.Insert(0, "0");
            }

            number = cleanNumber.ToString();

            for (i = 1; i <= 16; i++)
            {
                multiplier = 1 + (i % 2);
                digit = int.Parse(number.Substring(i - 1, 1));
                sum = digit * multiplier;
                if (sum > 9)
                    sum -= 9;
                total += sum;
            }

            return (total % 10 == 0);
        }

        // others to look at implementing

        // string type conversion 
        //http://msdn.microsoft.com/en-us/library/ff650837.aspx

        // relative date and times
        //http://msdn.microsoft.com/en-us/library/microsoft.practices.enterpriselibrary.validation.validators.relativedatetimevalidator_members(v=pandp.60).aspx

        // domain validator, string value in a list of strings
        //http://msdn.microsoft.com/en-us/library/ee745313(v=pandp.60).aspx



    }
}