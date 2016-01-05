using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Driven.Validation.Test
{
    [TestClass]
    public class StringTest
    {
        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ContainsCharacterGuard()
        {
            Guard.Against().ContainsCharacters(new[] { 'a', 'b', '~' }, "abcdefg", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ContainsCharacterStringGuard()
        {
            Guard.Against().ContainsCharacters("abc~", "abcdefg", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void ContainsCharacterGuard_Passes()
        {
            Guard.Against().ContainsCharacters(new[] { 'a', 'b', '~' }, "ziggi!@#", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void ContainsCharacterStringGuard_Passes()
        {
            Guard.Against().ContainsCharacters("abc~", "ziggi!@#", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void DoesContainCharacterGuard()
        {
            Guard.Against().DoesNotContainCharacters(new[] { 'a', 'b', '~' }, "ziggi!@#", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void DoesContainCharacterStringGuard()
        {
            Guard.Against().DoesNotContainCharacters("abc~", "ziggi!@#", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void DoesContainCharacterGuard_Passes()
        {
            Guard.Against().DoesNotContainCharacters(new[] { 'a', 'b', '~' }, "abcdefg", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void DoesContainCharacterStringGuard_Passes()
        {
            Guard.Against().DoesNotContainCharacters("abc~", "abcdefg", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringNullOrEmptyGuard_Passes()
        {
            Guard.Against().StringNullOrEmpty("abc123", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_Null()
        {
            Guard.Against().StringNullOrEmpty(null, "testParam").ThrowIfHasExceptions();
        }

        //[TestMethod, ExpectedException(typeof(GuardException))]
        //public void StringNullOrEmptyGuard_DbNull()
        //{
        //    Guard.Against().StringNullOrEmpty(DBNull.Value, "testParam").ThrowIfHasExceptions();
        //}

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_Empty()
        {
            Guard.Against().StringNullOrEmpty(string.Empty, "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_Spaces()
        {
            Guard.Against().StringNullOrEmpty("         ", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_Tab()
        {
            Guard.Against().StringNullOrEmpty("\t", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_Return()
        {
            Guard.Against().StringNullOrEmpty("\r", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_LineFeed()
        {
            Guard.Against().StringNullOrEmpty("\n", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNullOrEmptyGuard_CRLFT()
        {
            Guard.Against().StringNullOrEmpty("\r\n\t", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringLengthNotBetweenGuard()
        {
            Guard.Against().StringLengthNotBetween(50, 51, "abcde", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringLengthNotBetweenGuard_Passes()
        {
            Guard.Against().StringLengthNotBetween(0, 10, "abcde", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringLengthBetweenGuard()
        {
            Guard.Against().StringLengthBetween(0, 51, "abcde", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringLengthBetweenGuard_Passes()
        {
            Guard.Against().StringLengthBetween(50, 51, "abcde", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringNotNumericGuard_PassesInt()
        {
            Guard.Against().StringNotNumeric("123", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringNotNumericGuard_PassesIntNegative()
        {
            Guard.Against().StringNotNumeric("-123", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringNotNumericGuard_PassesDecimal()
        {
            Guard.Against().StringNotNumeric("12.3", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringNotNumericGuard_PassesComma()
        {
            Guard.Against().StringNotNumeric("1,1234.45", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod]
        public void StringNotNumericGuard_PassesDecimalNegative()
        {
            Guard.Against().StringNotNumeric("-12.3", "testParam").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNotNumericGuard_Alpha()
        {
            Guard.Against().StringNotNumeric("abc", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNotNumericGuard_AlphaAndNumeric()
        {
            Guard.Against().StringNotNumeric("10a", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNotNumericGuard_DollarSign()
        {
            Guard.Against().StringNotNumeric("$123.45", "testParam").ThrowIfHasExceptions();

        }

    }
}
