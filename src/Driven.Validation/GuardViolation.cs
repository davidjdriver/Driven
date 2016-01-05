using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Driven.Validation
{
    public class GuardViolation
    {
        public GuardViolation(string memberName, string message)
            : this(memberName, string.Empty, message)
        {
        }

        public GuardViolation(string memberName, string memberValue, string message)
        {
            this.MemberName = memberName;
            this.MemberValue = memberValue;
            this.Message = message;
        }

        public string Message { get; private set; }

        public string MemberName { get; private set; }

        public string MemberValue { get; private set; }

        public override string ToString()
        {
            return string.Format("Member {0}: Value: {1} Message: {2}", this.MemberName, this.MemberValue, this.Message);
        }

        public ValidationResult ToValidationResult()
        {
            return new ValidationResult(this.Message, new string[] { this.MemberName });
        }
    }
}
