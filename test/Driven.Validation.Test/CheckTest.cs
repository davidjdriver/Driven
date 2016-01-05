using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Driven.Validation;
using System.Linq;

namespace Driven.Validation.Test
{
    [TestClass]
    public class CheckTest
    {
        [TestMethod]
        public void DoesNotThrowException()
        {
            Guard.Against().Check();
            Guard.Against().Negative(50, "testVal").Check();
            Guard.Against()
                .Negative(50, "testVal")
                .Negative(50, "testVal2")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ThrowsGuardException()
        {
            Guard.Against().Negative(-5, "testVal").ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ThrowsGuardExceptionOnSecondFailure()
        {
            Guard.Against()
                .Negative(5, "otherValue")
                .Negative(-5, "testVal")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ThrowsGuardExceptionOnSecondFailureAfterCheckingFirst()
        {
            Guard.Against()
                .Negative(5, "otherValue")
                .Check()
                .Negative(-5, "testVal")
                .ThrowIfHasExceptions();
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void ThrowsGuardMultiException()
        {
            Guard.Against()
                .Negative(-5, "testVal")
                .Positive(5, "otherTest")
                .ThrowIfHasExceptions();
        }

        [TestMethod]
        public void GuardMultiExceptionCountMatch()
        {
            try
            {
                Guard.Against()
                .Negative(-5, "testVal")
                .Positive(5, "otherTest")
                .ThrowIfHasExceptions();
            }
            catch (GuardException ex)
            {

                Assert.AreEqual(2, ex.Data.Count);
            }

        }
    }
}
