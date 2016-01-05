using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Sample.Sql.InsertMasterDetails
{
    class CommandParam
    {
        private System.Data.DataTable lookupValueKeys;
        public string SomeValue { get; set; }
        public string OtherValue { get; set; }
        public System.Data.DataTable LookupValueKeys
        {
            get
            {
                return this.lookupValueKeys;
            }
        }

        public CommandParam()
        {
            this.lookupValueKeys = new System.Data.DataTable();
            this.lookupValueKeys.Columns.Add("LookupValueKey", typeof(int));
        }

        public void AddLookupKey(int val)
        {
            this.lookupValueKeys.Rows.Add(val);

        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("SomeValue: {0}, ", this.SomeValue);
            sb.AppendFormat("OterValue: {0}, ", this.OtherValue);
            sb.Append("ValueLookupKeys: ");
            foreach (System.Data.DataRow row in this.lookupValueKeys.Rows)
            {
                sb.Append(row["LookupValueKey"].ToString() + ", ");
            }

            return sb.ToString();
        }

    }
}
