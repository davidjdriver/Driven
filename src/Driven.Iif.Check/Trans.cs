using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Iif.Check
{
    public class Trans
    {
        public string Account { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public decimal AmountPaid { get; set; }

        public string DocNum { get; set; }

        public bool EFT { get; set; }

        public string Name { get; set; }

        public string OffsetAccount { get; set; }

        public DateTime PayDate { get; set; }

        public string TransactionClass { get; set; }
    }
}