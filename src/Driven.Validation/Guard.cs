// Found the patter for doing this here:
// http://blog.getpaint.net/2008/12/06/a-fluent-approach-to-c-parameter-validation/

namespace Driven.Validation
{
    using System;
    using System.Collections.Generic;

    public class Guard
    {
        public static Guard Against()
        {
            return new Guard();
        }

        private Guard()
        {
            this.HasViolations = false;
            this.HaultGuardEvaluation = false;
            this.guardViolations = new List<GuardViolation>();
        }

        public bool HaultGuardEvaluation { get; set; }

        public bool HasViolations { get; private set; }

        private List<GuardViolation> guardViolations;

        public IEnumerable<GuardViolation> GuardViolations { get { return this.guardViolations; } }

        public Guard AddViolation(GuardViolation guardViolation)
        {
            this.HasViolations = true;
            this.guardViolations.Add(guardViolation);
            return this;
        }
    }
}