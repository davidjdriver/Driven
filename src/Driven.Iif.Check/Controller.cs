using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Iif.Check
{
    public class Controller
    {
        /// <summary>
        /// This is a sample of that I did in the past
        /// </summary>
        public void BuildAFile()
        {
            //var checkIifRecs = this.ParseIifRecord(paymentRecs.Where(pr => pr.PaymentType == "CHECK"), config);
            var checkIifRecs = this.ParseIifRecord().ToArray();
            var checkPayments = checkIifRecs.Length;
            var checkAmount = checkIifRecs.Sum(c => c.AmountPaid);
            if (checkPayments > 0)
            {
                var checkPath = @"";
                var iifBuilder = new IifBuilder();
                string outText;
                outText = iifBuilder.BuildCheckFile(checkIifRecs);
                System.IO.File.WriteAllText(checkPath, outText);
            }
        }

        IEnumerable<Trans> ParseIifRecord() // there should be a param for header and detail values to map here 
        {
            yield return new Trans();
        }
    }
}
