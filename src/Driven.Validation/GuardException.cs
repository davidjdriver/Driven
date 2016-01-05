using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Validation
{
    [Serializable()]
    public class GuardException : Exception, ISerializable
    {
        public GuardException()
        {

        }

        public GuardException(string message)
            : base(message)
        {

        }

        public GuardException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public GuardException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public void AddGuardViolationsToData(IEnumerable<GuardViolation> guardViolations)
        {
            int i = 1;
            foreach (var item in guardViolations)
            {
                this.Data.Add(string.Format("Violation_{0}", i++), item.ToString());
            }
        }
    }

}
