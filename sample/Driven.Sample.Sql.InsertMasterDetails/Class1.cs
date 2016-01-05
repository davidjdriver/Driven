using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driven.Sample.Sql.InsertMasterDetails
{
    /// <summary>
    /// This can be done with dapper too but I figured out how to do this once and I don't want to lose it
    /// </summary>
    public class Class1
    {
        public static void Example()
        {
            try
            {
                //doSomethingInteresting(10000, 0); // looping and building commands
                doSomethingElse(10000, 0); // using a data table and data adaptor
            }
            catch (Exception ex)
            {
                Console.WriteLine("There has been an exception:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("The application will not close");
            }
            finally
            {
                Console.WriteLine("The application is complete. Press enter to close.");
                Console.ReadLine();

            }
        }



        private static void doSomethingElse(int datasetSize, int batchSize)
        {
            var myCommandParams = Class1.GetXCommandParams(datasetSize);
            var myTimer = Stopwatch.StartNew();
            using (var sqlConnection = new SqlConnection("Data Source=ddriverl7;Initial Catalog=BatchInsertTest;Integrated Security=True;Connection Timeout=900; MultipleActiveResultSets=True"))
            using (var sqlCommand = new SqlCommand("dbo.InsertMasterDetail", sqlConnection))
            using (var dataTable = new DataTable())
            using (var dataAdaptor = new SqlDataAdapter())
            {
                // set up the data table
                dataTable.Columns.Add("SomeValue", typeof(string));
                dataTable.Columns.Add("OtherValue", typeof(string));
                dataTable.Columns.Add("LookupValueList", typeof(System.Data.DataTable));
                // fill the data table
                foreach (var item in myCommandParams)
                {
                    dataTable.Rows.Add(item.SomeValue, item.OtherValue, item.LookupValueKeys);
                }

                // set up the adaptor for the procedure 
                sqlCommand.CommandType = CommandType.StoredProcedure;
                dataAdaptor.InsertCommand = sqlCommand;
                dataAdaptor.InsertCommand.Parameters.Add("SomeValue", SqlDbType.VarChar, 50, "SomeValue");
                dataAdaptor.InsertCommand.Parameters.Add("OtherValue", SqlDbType.VarChar, 50, "OtherValue");
                dataAdaptor.InsertCommand.Parameters.Add("LookupValueList", SqlDbType.Structured);
                // call the inserts
                sqlConnection.Open();
                myTimer.Reset();
                myTimer.Start();
                dataAdaptor.Update(dataTable);
                myTimer.Stop();
            }

            Console.WriteLine(string.Format("Data Adaptor executed {0} inserts in {1} command batches in {2} ms.", datasetSize, batchSize, myTimer.ElapsedMilliseconds));
        }

        private static void doSomethingInteresting(int datasetSize, int batchSize)
        {
            var myCommandParams = Class1.GetXCommandParams(datasetSize);

            //foreach (var item in myCommandParams)
            //{
            //    Console.WriteLine(item.ToString());
            //}
            var myTimer = Stopwatch.StartNew();
            using (var sqlConnection = new SqlConnection("get a sql database somewhere and create the objects in the text file"))
            using (var sqlCommand = new SqlCommand("dbo.InsertMasterDetail", sqlConnection))

            {
                sqlConnection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("SomeValue", SqlDbType.VarChar);
                sqlCommand.Parameters.Add("OtherValue", SqlDbType.VarChar);
                sqlCommand.Parameters.Add("LookupValueList", SqlDbType.Structured);
                foreach (var item in myCommandParams)
                {
                    sqlCommand.Parameters["SomeValue"].Value = item.SomeValue;
                    sqlCommand.Parameters["OtherValue"].Value = item.OtherValue;
                    sqlCommand.Parameters["LookupValueList"].Value = item.LookupValueKeys;
                    sqlCommand.ExecuteNonQuery();
                }
            }

            myTimer.Stop();
            Console.WriteLine(string.Format("Executed {0} inserts in {1} command batches in {2} ms.", datasetSize, batchSize, myTimer.ElapsedMilliseconds));
        }

        //private static SqlCommand GetCommandForCommandParams(CommandParam cParam, SqlConnection sqlCon)
        //{
        //    var sqlCommand = new SqlCommand("dbo.InsertMasterDetail", sqlCon);
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.Parameters.Add("SomeValue", SqlDbType.VarChar);
        //    sqlCommand.Parameters.Add("OtherValue", SqlDbType.VarChar);
        //    sqlCommand.Parameters.Add("LookupValueList", SqlDbType.Structured);
        //    sqlCommand.Parameters["SomeValue"].Value = cParam.SomeValue;
        //    sqlCommand.Parameters["OtherValue"].Value = cParam.OtherValue;
        //    sqlCommand.Parameters["LookupValueList"].Value = cParam.LookupValueKeys;
        //    return sqlCommand;
        //}

        private static List<CommandParam> GetXCommandParams(int commandCount)
        {
            List<CommandParam> commandParams = new List<CommandParam>();
            CommandParam commandParam;
            for (int i = 0; i < commandCount; i++)
            {
                commandParam = new CommandParam()
                {
                    SomeValue = string.Format("Some Value: {0}", i),
                    OtherValue = string.Format("Other Value: {0}", i)
                };

                commandParam.AddLookupKey(1);
                commandParam.AddLookupKey(2);
                commandParam.AddLookupKey(3);
                commandParam.AddLookupKey(4);
                commandParam.AddLookupKey(5);
                commandParams.Add(commandParam);
            }

            return commandParams;
        }
    }
}
