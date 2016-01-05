using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Driven.Validation.Test
{
    [TestClass]
    public class DateTest
    {

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void DefaultGuard()
        {
            var d1 = new DateTime();
            Guard.Against().Default(d1, "d1")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void GreaterThanGuard()
        {
            var d1 = new DateTime(2014, 1, 1);
            var d2 = new DateTime(2013, 1, 1);
            Guard.Against().GreaterThan(d2, d1, "d1")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void LessThanGuard()
        {
            var d1 = new DateTime(2014, 1, 1);
            var d2 = new DateTime(2013, 1, 1);
            Guard.Against().GreaterThan(d2, d1, "d2")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void BetweenGuard()
        {
            var d1 = new DateTime(2014, 1, 1);
            var d2 = new DateTime(2013, 1, 1);
            var d3 = new DateTime(2015, 1, 1);
            Guard.Against().Between(d1, d2, d3, "d1")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotBetweenLessThanGuard()
        {
            var d1 = new DateTime(2015, 1, 1);
            var d2 = new DateTime(2014, 1, 1);
            var d3 = new DateTime(2013, 1, 1);
            Guard.Against().Between(d1, d2, d3, "d1")
                .ThrowIfHasExceptions();
        }

        public void NotBetweenGreaterThanGuard()
        {
            var d1 = new DateTime(2015, 1, 1);
            var d2 = new DateTime(2014, 1, 1);
            var d3 = new DateTime(2016, 1, 1);
            Guard.Against().Between(d1, d2, d3, "d1")
                .ThrowIfHasExceptions();
        }
    }
}
