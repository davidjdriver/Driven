using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Driven.Validation
{
    public static class GuardExtensions
    {
        private static Regex regexEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        // used if you want to stop evaluation after failing initial checks
        public static Guard Check(this Guard guard)
        {
            guard.HaultGuardEvaluation = guard.HasViolations;
            return guard;
        }

        public static IEnumerable<ValidationResult> GetValidationResults(this Guard guard)
        {
            foreach (var item in guard.GuardViolations)
            {
                yield return item.ToValidationResult();
            }
        }

        public static void ThrowIfHasExceptions(this Guard guard)
        {
            if (!guard.HasViolations) return;
            var ex = new GuardException("Data validation has failed. See exception data for violation details.");
            ex.AddGuardViolationsToData(guard.GuardViolations);
            throw ex;
        }

        public static Guard Null<T>(this Guard guard, T memberValue, string memberName)
             where T : class
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue == null || DBNull.Value.Equals(memberValue))
            {
                return guard.AddViolation(new GuardViolation(memberName, "Should not be null but has a null or equivalent value."));
            }

            return guard;
        }

        public static Guard Default<T>(this Guard guard, T memberValue, string memberName)
            where T : struct
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.Equals(default(T)))
            {
                return guard.AddViolation(new GuardViolation(memberName, "Structure has default values and expected a non-default structure."));
            }

            return guard;
        }

        public static Guard NullNullable<T>(this Guard guard, Nullable<T> memberValue, string memberName)
             where T : struct, IComparable<T>
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (!memberValue.HasValue)
            {
                return guard.AddViolation(new GuardViolation(memberName, "Should not be null but has a null or equivalent value."));
            }

            return guard;
        }

        public static Guard Negative<T>(this Guard guard, T memberValue, string memberName)
            where T : struct, IConvertible, IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.CompareTo(0) < 0)
            {
                var msg = string.Format("Expected positive value but received {0}", memberValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard Positive<T>(this Guard guard, T memberValue, string memberName)
            where T : struct, IConvertible, IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.CompareTo(0) > 0)
            {
                var msg = string.Format("Expected negative value but received {0}", memberValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard GreaterThan<T>(this Guard guard, T compareValue, T memberValue, string memberName)
            where T : struct, IConvertible, IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.CompareTo(compareValue) > 0)
            {
                var msg = string.Format("Expected value to be less than {0}", compareValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard LessThan<T>(this Guard guard, T compareValue, T memberValue, string memberName)
            where T : struct, IConvertible, IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.CompareTo(compareValue) < 0)
            {
                var msg = string.Format("Expected value to be greater than {0}", compareValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard Between<T>(this Guard guard, T startValue, T endValue, T memberValue, string memberName)
            where T : struct, IConvertible, IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.CompareTo(startValue) > 0 || memberValue.CompareTo(endValue) < 0)
            {
                var msg = string.Format("Expected value to not be between {0} and {1}", startValue, endValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard NotBetween<T>(this Guard guard, T startValue, T endValue, T memberValue, string memberName)
            where T : IComparable
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (!(memberValue.CompareTo(startValue) >= 0 && memberValue.CompareTo(endValue) <= 0))
            {
                var msg = string.Format("Expected value to be between {0} and {1}", startValue, endValue);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard StringNullOrEmpty(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (StringIsNull(memberValue))
            {
                return guard.AddViolation(new GuardViolation(memberName, "String value expected to not be null or empty."));
            }

            return guard;
        }

        public static Guard StringLengthBetween(this Guard guard, int minLength, int maxLengh, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            var valueLen = memberValue.Length;
            if (((valueLen >= minLength) && (valueLen <= maxLengh)))
            {
                var msg = string.Format("string length {1} not between the required range of {1} and {2}", valueLen, minLength, maxLengh);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard StringLengthNotBetween(this Guard guard, int minLength, int maxLengh, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            var valueLen = memberValue.Length;
            if (!((valueLen >= minLength) && (valueLen <= maxLengh)))
            {
                var msg = string.Format("string length {1} not between the required range of {1} and {2}", valueLen, minLength, maxLengh);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard StringNotNumeric(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            double outVal;
            if (!(double.TryParse(memberValue, out outVal)))
            {
                return guard.AddViolation(new GuardViolation(memberName, memberValue, "String expected to be numeric."));
            }

            return guard;
        }

        public static Guard StringNumeric(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            double outVal;
            if (double.TryParse(memberValue, out outVal))
            {
                return guard.AddViolation(new GuardViolation(memberName, memberValue, "Expected string to not be numeric."));
            }

            return guard;
        }

        public static Guard NotEmail(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (string.IsNullOrWhiteSpace(memberValue) || !regexEmail.IsMatch(memberValue))
            {
                return guard.AddViolation(new GuardViolation(memberName, memberValue, "Expected value to be a valid email address."));
            }

            return guard;
        }

        public static Guard NotUrl(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            Uri uri;
            bool validUri = false;
            if (Uri.TryCreate(memberValue, UriKind.Absolute, out uri))
            {
                validUri = (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            }

            if (!validUri)
            {
                return guard.AddViolation(new GuardViolation(memberName, memberValue, "Expected a valid HTTP or HTTPS URL"));
            }

            return guard;
        }

        public static Guard MatchesRegex(this Guard guard, string pattern, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            Regex exp = new Regex(pattern);
            if (string.IsNullOrWhiteSpace(memberValue) || exp.IsMatch(memberValue))
            {
                var msg = string.Format("Expected value to not match regex: {0}", pattern);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard NotMatchesRegex(this Guard guard, string pattern, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            Regex exp = new Regex(pattern);
            if (string.IsNullOrWhiteSpace(memberValue) || !exp.IsMatch(memberValue))
            {
                var msg = string.Format("Expected value to match regex: {0}", pattern);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        //.ArgumentOutOfRange((collection.Count == 1), "arg1", "collection must not have one item")
        public static Guard ArgumentOutOfRange(this Guard guard, bool condition, string memberName, string message)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (condition)
            {
                return guard.AddViolation(new GuardViolation(memberName, message));
            }

            return guard;
        }

        // like above but without the parameter name parameter
        public static Guard InvalidOperation(this Guard guard, bool condition, string message)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (condition)
            {
                return guard.AddViolation(new GuardViolation(string.Empty, message));
            }

            return guard;
        }

        public static Guard StringIsNotEnum(this Guard guard, Type enumType, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            // check we are comparing an enum
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType parameter should be an enum", "enumType");
            }

            // find out if the string is in the enum
            if (!Enum.IsDefined(enumType, memberValue))
            {
                var msg = string.Format("Value expected to be a member of enum {0}", enumType);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard IntIsNotEnum(this Guard guard, Type enumType, int memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            // check we are comparing an enum
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType parameter should be an enum", "enumType");
            }

            // find out if the string is in the enum
            if (!Enum.IsDefined(enumType, memberValue))
            {
                var msg = string.Format("Value expected to be a member of enum {0}", enumType);
                return guard.AddViolation(new GuardViolation(memberName, memberValue.ToString(), msg));
            }

            return guard;
        }

        public static Guard InvaidCreditCardNumber(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (!IsValidCreditCard(memberValue))
            {
                return guard.AddViolation(new GuardViolation(memberName, memberValue, "Expected valid credit card number."));
            }

            return guard;
        }

        public static Guard ContainsCharacters(this Guard guard, char[] testCharacters, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.IndexOfAny(testCharacters) != -1)
            {
                var msg = string.Format("Value expected to not contain the following characters {0}.", testCharacters);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard ContainsCharacters(this Guard guard, string testCharacters, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.IndexOfAny(testCharacters.ToCharArray()) != -1)
            {
                var msg = string.Format("Value expected to not contain the following characters {0}.", testCharacters);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard DoesNotContainCharacters(this Guard guard, char[] testCharacters, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.IndexOfAny(testCharacters) == -1)
            {
                var msg = string.Format("Value expected to contain at least one of the following characters {0}.", testCharacters);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard DoesNotContainCharacters(this Guard guard, string testCharacters, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            if (memberValue.IndexOfAny(testCharacters.ToCharArray()) == -1)
            {
                var msg = string.Format("Value expected to contain at least one of the following characters {0}.", testCharacters);
                return guard.AddViolation(new GuardViolation(memberName, memberValue, msg));
            }

            return guard;
        }

        public static Guard WeakPassword(this Guard guard, string memberValue, string memberName)
        {
            if (guard.HaultGuardEvaluation) return guard;

            throw new NotImplementedException();
        }

        private static bool StringIsNull(string memberValue)
        {
            return (String.IsNullOrWhiteSpace(memberValue) || DBNull.Value.Equals(memberValue));
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