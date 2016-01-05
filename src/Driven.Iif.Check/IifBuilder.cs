using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Iif.Check
{
    public class IifBuilder
    {
        public string BuildCheckFile(Trans[] recs)
        {
            var sb = new StringBuilder();
            sb.AppendLine(this.GetIifHeader());
            foreach (var item in recs)
            {
                sb.AppendLine(FormatTrns(item));
                sb.AppendLine(FormatSpl(item)); // don't forget to check into flipping values
                sb.AppendLine("ENDTRNS");
            }

            return sb.ToString();
        }

        private string FormatSpl(Trans rec)
        {
            //be sure ToString invert amounts and tran types
            // as per the orig payout project
            var sb = new System.Text.StringBuilder();
            sb.Append("SPL\t\tCHECK\t");
            sb.Append(rec.PayDate.ToString("M/dd/yyyy"));
            sb.Append("\t");
            sb.Append(rec.OffsetAccount);
            sb.Append("\t\t");
            sb.Append(rec.TransactionClass);
            sb.Append("\t");
            sb.Append(rec.AmountPaid.ToString("F2"));
            sb.Append("\t\t\t\t\t\t\t\t");
            sb.Append("N");
            sb.Append("\t\t\t\t");
            sb.Append("0\t");
            return sb.ToString();
        }

        private string FormatTrns(Trans rec)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("TRNS\t\tCHECK\t");
            sb.Append(rec.PayDate.ToString("M/dd/yyyy"));
            sb.Append("\t");
            sb.Append(rec.Account);
            sb.Append("\t");
            sb.Append(rec.Name);
            sb.Append("\t");
            sb.Append(rec.TransactionClass);
            sb.Append("\t");
            sb.Append((rec.AmountPaid * -1).ToString("F2"));
            sb.Append("\t");
            if (rec.EFT)
            {
                sb.Append("EFT");
            }
            else
            {
                sb.Append(rec.DocNum);
            }

            sb.Append("\t\t\t");
            if (rec.EFT)
            {
                sb.Append("N");
            }
            else
            {
                sb.Append("Y");
            }

            sb.Append("\t");
            sb.Append("N\t");
            sb.Append("\t\t\t\t\t\t\t\t\t");
            sb.Append(rec.Address1);
            sb.Append("\t");
            sb.Append(rec.Address2);
            sb.Append("\t");
            sb.Append(rec.Address3);
            sb.Append("\t\t");
            return sb.ToString();
        }

        private string GetIifHeader()
        {
            return @"!TRNS	TRNSID	TRNSTYPE	DATE	ACCNT	NAME	CLASS	AMOUNT	DOCNUM	MEMO	CLEAR	TOPRINT	NAMEISTAXABLE	DUEDATE	TERMS	PAYMETH	SHIPVIA	SHIPDATE	REP	FOB	PONUM	INVMEMO	ADDR1	ADDR2	ADDR3	ADDR4	ADDR5
!SPL	SPLID	TRNSTYPE	DATE	ACCNT	NAME	CLASS	AMOUNT	DOCNUM	MEMO	CLEAR	QNTY	PRICE	INVITEM	PAYMETH	TAXABLE	EXTRA	VATCODE	VATRATE	VATAMOUNT	VALADJ
!ENDTRNS";
        }
    }
}