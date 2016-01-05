using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Driven.Validation.Test
{
    [TestClass]
    public class CreditCardTest
    {
        //https://www.paypalobjects.com/en_US/vhelp/paypalmanager_help/credit_card_numbers.htm
        private string [] goodCCNumbers = {"30569309025904", "3530111333300000", "3566002020360505", "371449635398431", "378282246310005", "378734493671000", "38520000023237", "4012888888881881", "4111111111111111", "4222222222222", "5019717010103742", "5105105105105100", "5555555555554444", "5610591081018250", "6011000990139424", "6011111111111117", "6331101999990016", "76009244561",};

        [TestMethod]
        public void NotCreditCardGuardValidPasses()
        {
            foreach (var item in this.goodCCNumbers)
            {
                Guard.Against().InvaidCreditCardNumber(goodCCNumbers[1], "testParam").ThrowIfHasExceptions();
            }            
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidEmpty()
        {
            Guard.Against().InvaidCreditCardNumber("", "testParam").ThrowIfHasExceptions();
            
        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidNull()
        {
            Guard.Against().InvaidCreditCardNumber(null, "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidAlpha()
        {
            Guard.Against().InvaidCreditCardNumber("123Abc", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidOtherChar()
        {
            Guard.Against().InvaidCreditCardNumber("123~", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidTooShort()
        {
            Guard.Against().InvaidCreditCardNumber("123", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidTooLong()
        {
            Guard.Against().InvaidCreditCardNumber("12345678901234567890", "testParam").ThrowIfHasExceptions();

        }

        [TestMethod, ExpectedException(typeof(GuardException))]
        public void NotCreditCardValidDoesntAddUp()
        {
            Guard.Against().InvaidCreditCardNumber("40569309025904", "testParam").ThrowIfHasExceptions();

        }
        

    }
}
