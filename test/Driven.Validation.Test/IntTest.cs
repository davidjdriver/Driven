using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Driven.Validation.Test
{
    [TestClass]
    public class IntTest
    {
        [TestMethod, ExpectedException(typeof(GuardException))]
        public void DefaultGuard()
        {
            int i = new int();
            Guard.Against().Default(i, "i").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NegativeGuard()
        {
            int i = -1;
            Guard.Against().Negative(i, "i").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void PositiveGuard()
        {
            int i = 1;
            Guard.Against().Positive(i, "i").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void GreaterThanGuard()
        {
            int i = 1;
            int j = 2;
            Guard.Against().GreaterThan(i, j, "j").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void LessThanGuard()
        {
            int i = 2;
            int j = 1;
            Guard.Against().LessThan(i, j, "j").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void BetweenGuard()
        {
            int i = 1;
            int j = 10;
            int k = 5;
            Guard.Against().Between(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void BetweenMinIsSameGuard()
        {
            int i = 1;
            int j = 10;
            int k = 1;
            Guard.Against().Between(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void BetweenMaxIsSameGuard()
        {
            int i = 1;
            int j = 10;
            int k = 10;
            Guard.Against().Between(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotBetweenGreaterThanGuard()
        {
            int i = 1;
            int j = 10;
            int k = 50;
            Guard.Against().NotBetween(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotBetweenLessThanGuard()
        {
            int i = 1;
            int j = 10;
            int k = 0;
            Guard.Against().NotBetween(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void NotBetweenMinIsSameGuard()
        {
            int i = 1;
            int j = 10;
            int k = 1;
            Guard.Against().NotBetween(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void NotBetweenMaxIsSameGuard()
        {
            int i = 1;
            int j = 10;
            int k = 10;
            Guard.Against().NotBetween(i, j, k, "k").ThrowIfHasExceptions();
        }

        [TestMethod]
        public void NotBetweenInTheMiddleGuard()
        {
            int i = 1;
            int j = 10;
            int k = 5;
            Guard.Against().NotBetween(i, j, k, "k").ThrowIfHasExceptions();
        }
    }
}
