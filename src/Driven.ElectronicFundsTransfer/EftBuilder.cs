using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.ElectronicFundsTransfer
{
    public class EftBuilder
    {

        private decimal batchAmount;
        private long batchHash;
        private long batchNumber;
        private long batchTransactions;
        private bool complete;
        private EftHeader currentHeader;
        private decimal fileAmount;
        private long fileHash;
        private long fileLines;
        private StringBuilder fileText;
        private long fileTransactions;

        public EftBuilder(EftHeader header)
        {
            this.currentHeader = header;
            this.complete = false;
            this.fileLines = 0;
            this.fileHash = 0;
            this.fileTransactions = 0;
            this.fileAmount = 0m;
            this.batchNumber = 0;
            this.batchHash = 0;
            this.batchTransactions = 0;
            this.batchAmount = 0m;
            this.fileText = new StringBuilder();
        }

        public string BuildFile(EftTransaction[] recs)
        {
            if (this.complete)
            {
                throw new Exception("This ACH file has been saved. No further data can be added");
            }

            this.AddLineToFileText(this.FormatFileHeader());
            this.AddLineToFileText(this.FormatBatchHeader());
            foreach (var item in recs)
            {
                this.AddLineToFileText(this.FormatTrans(item));
            }
            this.AddLineToFileText(this.FormatOfsettingTransaction());
            this.AddLineToFileText(this.FormatBatchControl());
            this.AddLineToFileText(this.FormatFileControl());
            this.complete = true;
            return this.fileText.ToString();
        }

        public string FormatBatchControl()
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();

            // record type 
            sbLine.Append("8");

            // Service class code 
            sbLine.Append("200");

            // Entry/Addenda count, tally of all transactions and addendas 
            sbLine.Append(this.batchTransactions.ToString().PadLeft(6, Convert.ToChar(48)));

            // Entry hash 
            if (this.batchHash.ToString().Length > 10)
            {
                this.batchHash = Convert.ToInt64(this.batchHash.ToString().Substring(0, 10));
            }
            sbLine.Append(this.batchHash.ToString().PadLeft(10, Convert.ToChar(48)));

            // Total debit amounts 
            sbLine.Append(FormatAchCurrency(this.batchAmount, 12));

            // Total credits 
            sbLine.Append(FormatAchCurrency(this.batchAmount, 12));

            // Company identification, EIN number preceeded by a 1 
            if (this.currentHeader.CompanyIdentification.Length == 9)
            {
                sbLine.Append("1");
                sbLine.Append(this.currentHeader.CompanyIdentification);
            }
            else
            {
                sbErrors.AppendLine("Company identifier too long: " + this.currentHeader.CompanyIdentification);
            }

            // Message authentication code, blank 
            sbLine.Append(new string(' ', 19));

            // Reserved, blank 
            sbLine.Append(new string(' ', 6));

            // Originating DFI ID, bank's routing number without end check digit 
            if (this.currentHeader.RoutingNumber.Length == 9)
            {
                sbLine.Append(this.currentHeader.RoutingNumber.Substring(0, 8));
            }
            else
            {
                sbErrors.AppendLine("Routing number too long: " + this.currentHeader.RoutingNumber);
            }

            // Batch number 
            sbLine.Append(this.batchNumber.ToString().PadLeft(7, Convert.ToChar(48)));

            if (sbErrors.Length == 0)
            {
                return sbLine.ToString();
            }
            else
            {
                throw new Exception("Data validation error while writing ACH file. Error(s): " + sbErrors.ToString());
            }
        }

        public string FormatBatchHeader()
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();
            // reset batch accums 
            this.batchNumber += 1;
            this.batchTransactions = 0;
            this.batchHash = 0;
            this.batchAmount = 0;

            // write the header 

            // Record type 
            sbLine.Append("5");

            // Service class code 
            sbLine.Append("200");

            // Company name 
            if (this.currentHeader.CompanyName.Length <= 16)
            {
                sbLine.Append(this.currentHeader.CompanyName.PadRight(16));
            }
            else
            {
                sbErrors.AppendLine("Company name is too long: " + this.currentHeader.CompanyName);
            }

            // Company discretionary data 
            if (this.currentHeader.CompanyDiscretionaryData.Length <= 20)
            {
                sbLine.Append(this.currentHeader.CompanyDiscretionaryData.PadRight(20));
            }
            else
            {
                sbErrors.AppendLine("Company discretionary data too long: " + this.currentHeader.CompanyDiscretionaryData);
            }

            // Company identification, "1" + EIN 
            if (this.currentHeader.CompanyIdentification.Length == 9)
            {
                sbLine.Append("1");
                sbLine.Append(this.currentHeader.CompanyIdentification);
            }
            else
            {
                sbErrors.AppendLine("Company identifier too long: " + this.currentHeader.CompanyIdentification);
            }

            // Standard entry class 
            sbLine.Append("PPD");

            // Company entry description 
            if (this.currentHeader.CompanyEntryDescription.Length <= 10)
            {
                sbLine.Append(this.currentHeader.CompanyEntryDescription.PadRight(10));
            }
            else
            {
                sbErrors.AppendLine("Entry description too long: " + this.currentHeader.CompanyEntryDescription);
            }

            // Company descriptive date 
            sbLine.Append(this.currentHeader.EffectiveDate.ToString("MMddyy"));

            // Effective entry date 
            sbLine.Append(this.currentHeader.EffectiveDate.ToString("yyMMdd"));

            // Settlement date 
            sbLine.Append(new string(' ', 3));

            // Originator status code 
            sbLine.Append("1");

            // Originating DFI identification, bank routing number without ending check digit 
            if (this.currentHeader.DfiId.Length == 8)
            {
                sbLine.Append(this.currentHeader.DfiId);
            }
            else
            {
                sbErrors.AppendLine("DFIID incorrect length: " + this.currentHeader.DfiId);
            }

            // Batch number 
            sbLine.Append(this.batchNumber.ToString().PadLeft(7, Convert.ToChar(48)));

            if (sbErrors.Length == 0)
            {
                return sbLine.ToString();
            }
            else
            {
                throw new Exception("Data validation error while writing ACH file. Error(s): " + sbErrors.ToString());
            }
        }

        public string FormatFileControl()
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();

            // add the control record 

            // Record type 
            sbLine.Append("9");

            // Batch count 
            sbLine.Append(this.batchNumber.ToString().PadLeft(6, Convert.ToChar(48)));

            // Block count 
            sbLine.Append(Convert.ToInt32((this.fileLines * 94) / 940).ToString().PadLeft(6, Convert.ToChar(48)));

            // Entry/Addenda count 
            sbLine.Append(this.fileTransactions.ToString().PadLeft(8, Convert.ToChar(48)));

            // Entry hash 
            if (this.fileHash.ToString().Length > 10)
            {
                this.fileHash = Convert.ToInt64(this.fileHash.ToString().Substring(0, 10));
            }
            sbLine.Append(this.fileHash.ToString().PadLeft(10, Convert.ToChar(48)));

            // Total debits 
            sbLine.Append(FormatAchCurrency(this.fileAmount, 12));

            // Total credits 
            sbLine.Append(FormatAchCurrency(this.fileAmount, 12));

            // reserved, blank 
            sbLine.Append(new string(' ', 39));

            if (sbErrors.Length == 0)
            {
                return sbLine.ToString();
            }
            else
            {
                throw new Exception("Data validation error while writing ACH file. Error(s): " + sbErrors.ToString());
            }
        }

        public string FormatFileHeader()
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();
            // record type 
            sbLine.Append("1");
            // PriorityCode 
            sbLine.Append("01");

            // Immediate destination routing number 
            if (this.currentHeader.DestinationRoutingNumber.Length == 9)
            {
                sbLine.Append(" " + this.currentHeader.DestinationRoutingNumber);
            }
            else
            {
                sbErrors.AppendLine("Immediate destination routing number not 9 characters");
            }

            // Immediate origin routing number 
            if (this.currentHeader.OriginRoutingnumber.Length == 9)
            {
                sbLine.Append(" " + this.currentHeader.OriginRoutingnumber);
            }
            else
            {
                sbErrors.AppendLine("Immediate origin routing number not 9 characters");
            }

            // FileCreationDate 
            sbLine.Append(this.currentHeader.EffectiveDate.ToString("yyMMdd"));

            // FileCreationTime 
            sbLine.Append(DateTime.Now.ToString("hhmm"));

            // FileIdModifier 
            sbLine.Append("H");

            // RecordSize 
            sbLine.Append("094");

            // Blocking factor 
            sbLine.Append("10");

            // Format code 
            sbLine.Append("1");

            // Immediate destination name 
            if (this.currentHeader.DestinationName.Length <= 23)
            {
                sbLine.Append(this.currentHeader.DestinationName.PadRight(23));
            }
            else
            {
                sbErrors.AppendLine("Immediate destination name not proper length: " + this.currentHeader.DestinationName);
            }

            // Immidiate origin name 
            if (this.currentHeader.OriginName.Length <= 23)
            {
                sbLine.Append(this.currentHeader.OriginName.PadRight(23));
            }
            else
            {
                sbErrors.AppendLine("Immediate origin name not proper length: " + this.currentHeader.OriginName);
            }

            // Reference code, not used according to documentation 
            sbLine.Append(new string(' ', 8));

            if (sbErrors.Length == 0)
            {
                return sbLine.ToString();
            }
            else
            {
                throw new Exception("Data validation error while writing ACH file. Error(s): " + sbErrors.ToString());
            }
        }

        public string FormatNewBatch()
        {
            var sbLine = new StringBuilder();
            return sbLine.ToString();
        }

        public string FormatOfsettingTransaction()
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();

            // Record type 
            sbLine.Append("6");

            // Transaction code 
            sbLine.Append(this.currentHeader.OffsettingTransactionCode);

            if (this.currentHeader.RoutingNumber.Length == 9)
            {
                // Receiving DFI id first eight digits of the bank routing number 
                sbLine.Append(this.currentHeader.RoutingNumber.Substring(0, 8));

                // Check digit, the 9th digit to the routing number. 
                sbLine.Append(this.currentHeader.RoutingNumber.Substring(8, 1));
            }
            else
            {
                sbErrors.AppendLine("Invalid length routing number: " + this.currentHeader.RoutingNumber);
            }

            // DFI account number 
            if (this.currentHeader.AccountNumber.ToString().Length <= 17)
            {
                sbLine.Append(this.currentHeader.AccountNumber.PadRight(17));
            }
            else
            {
                sbErrors.AppendLine("Invalid length account number: " + this.currentHeader.AccountNumber);
            }

            // Amount 
            sbLine.Append(FormatAchCurrency(this.batchAmount, 10));

            // Individual identification number (NABP for us) 
            sbLine.Append("0000000".Trim().PadLeft(7, (char)48).PadLeft(15));

            // Individual name 
            if (this.currentHeader.CompanyName.Trim().Length > 22)
            {
                this.currentHeader.CompanyName = this.currentHeader.CompanyName.Trim().Substring(0, 22);
            }
            sbLine.Append(this.currentHeader.CompanyName.Trim().PadRight(22));

            // Discretionary data 
            sbLine.Append(new string(' ', 2));

            // Addenda record indicator 
            sbLine.Append("0");

            // Trace number, first eight digits of our bank's routing number plus the transaction number 
            sbLine.Append(this.currentHeader.OriginRoutingnumber.Substring(0, 8));
            sbLine.Append((this.batchTransactions + 1).ToString().PadLeft(7, Convert.ToChar(48)));

            // Increment counters and hashes lines 
            this.batchTransactions += 1;
            this.fileTransactions += 1;

            // don't increment the money for these offsetting transactions 

            // hashes 
            this.batchHash += Convert.ToInt64(this.currentHeader.RoutingNumber.Substring(0, 8));
            this.fileHash += Convert.ToInt64(this.currentHeader.RoutingNumber.Substring(0, 8));

            return sbLine.ToString();
        }

        public string FormatTrans(EftTransaction rec)
        {
            var sbLine = new StringBuilder();
            var sbErrors = new System.Text.StringBuilder();

            // do all the rounding here so that hopefully the totals match 
            rec.Amount = decimal.Round(rec.Amount, 2);

            // Record type 
            sbLine.Append("6");

            // Transaction code 
            sbLine.Append(this.currentHeader.TransactionCode);

            if (rec.RoutingNumber.Length == 9)
            {
                // Receiving DFI id first eight digits of the bank routing number 
                sbLine.Append(rec.RoutingNumber.Substring(0, 8));

                // Check digit, the 9th digit to the routing number. 
                sbLine.Append(rec.RoutingNumber.Substring(8, 1));
            }
            else
            {
                sbErrors.AppendLine("Invalid length routing number: " + rec.RoutingNumber);
            }

            // DFI account number 
            if (rec.AccountNumber.ToString().Length <= 17)
            {
                sbLine.Append(rec.AccountNumber.PadRight(17));
            }
            else
            {
                sbErrors.AppendLine("Invalid length account number: " + rec.AccountNumber);
            }

            // Amount 
            sbLine.Append(FormatAchCurrency(rec.Amount, 10));

            // Individual identification number (NABP for us) 
            sbLine.Append(rec.IndividualIdNumber.Trim().PadLeft(7, (char)48).PadLeft(15));

            // Individual name 
            if (rec.IndividualName.Trim().Length > 22)
            {
                rec.IndividualName = rec.IndividualName.Trim().Substring(0, 22);
            }
            sbLine.Append(rec.IndividualName.Trim().PadRight(22));

            // Discretionary data 
            sbLine.Append(new string(' ', 2));

            // Addenda record indicator 
            sbLine.Append("0");

            // Trace number, first eight digits of our bank's routing number plus the transaction number 
            sbLine.Append(this.currentHeader.OriginRoutingnumber.Substring(0, 8));
            sbLine.Append((this.batchTransactions + 1).ToString().PadLeft(7, Convert.ToChar(48)));

            // Increment counters and hashes lines 
            this.batchTransactions += 1;
            this.fileTransactions += 1;

            // money 
            this.batchAmount += rec.Amount;
            this.fileAmount += rec.Amount;

            // hashes 
            this.batchHash += Convert.ToInt64(rec.RoutingNumber.Substring(0, 8));
            this.fileHash += Convert.ToInt64(rec.RoutingNumber.Substring(0, 8));

            if (sbErrors.Length == 0)
            {
                return sbLine.ToString();
            }
            else
            {
                throw new Exception("Data validation error while writing ACH file. Error(s): " + sbErrors.ToString());
            }
        }

        private void AddLineToFileText(string line)
        {
            if (line.Length == 94)
            {
                this.fileText.AppendLine(line);
                this.fileLines += 1;
            }
            else
            {
                throw new Exception("Invalid line length: " + line.Length.ToString() + " Here is the line: " + line);
            }
        }

        private string FormatAchCurrency(decimal dAmount, Int32 PadLength)
        {
            //Return CInt((dAmount * 100)).ToString.PadLeft(PadLength, Convert.ToChar(48))
            return (dAmount * 100).ToString(new string('0', PadLength));
        }
    }
}
