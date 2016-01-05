using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.ElectronicFundsTransfer
{
    public class EftHeader
    {
        public string AccountNumber { get; set; }

        public string CompanyDiscretionaryData { get; set; }

        public string CompanyEntryDescription { get; set; }

        public string CompanyIdentification { get; set; }

        public string CompanyName { get; set; }

        public string DfiId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string DestinationName { get; set; }

        public string DestinationRoutingNumber { get; set; }

        public string OriginName { get; set; }

        public string OriginRoutingnumber { get; set; }

        public string OffsettingTransactionCode { get; set; }

        public string RoutingNumber { get; set; }

        public string TransactionCode { get; set; }
    }
}
