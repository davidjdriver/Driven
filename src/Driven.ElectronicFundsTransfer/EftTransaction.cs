using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.ElectronicFundsTransfer
{
    public class EftTransaction
    {
        public string AccountNumber { get; set; }

        public decimal Amount { get; set; }

        public string IndividualIdNumber { get; set; }

        public string IndividualName { get; set; }

        public string RoutingNumber { get; set; }
    }
}
