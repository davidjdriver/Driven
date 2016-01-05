using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Driven.Validation.Test
{
    public enum WeekDays { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

    [TestClass]
    public class EnumTest
    {
        [TestMethod, ExpectedException(typeof(GuardException))]
        public void StringNotInEnumGuard()
        {
            Guard.Against().StringIsNotEnum(typeof(WeekDays), "Sundayy", "testParam")
                .ThrowIfHasExceptions();
        }

        [TestMethod]
        public void StringInEnumGuard()
        {
            Guard.Against().StringIsNotEnum(typeof(WeekDays), "Sunday", "testParam")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void IntNotInEnumGuard()
        {
            Guard.Against().IntIsNotEnum(typeof(WeekDays), 100, "testParam")
                .ThrowIfHasExceptions();
        }

        [TestMethod]
        public void IntInEnumGuard()
        {
            Guard.Against().IntIsNotEnum(typeof(WeekDays), 1, "testParam")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void IntNotInEnumTypeNotEnumGuard()
        {
            Guard.Against()
                .IntIsNotEnum(typeof(String), 100, "testParam")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void StringNotInEnumTypeNotEnumGuard()
        {
            Guard.Against()
                .StringIsNotEnum(typeof(String), "testvalue", "testParam")
                .ThrowIfHasExceptions();
        }
    }
}
