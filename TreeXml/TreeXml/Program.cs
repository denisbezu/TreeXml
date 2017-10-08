

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Threading.Tasks;
using TreeXmlLibrary;

namespace TreeXml
{


    class Program
    {


        static void Main(string[] args)
        {
            //DisplayResult();
            Node root = new Node("root");
            Node ch1 = new Node("ch1");
            Node ch2 = new Node("ch2");
            root.AddChild(ch1);
            root.AddChild(ch2);
           // DisplayResult();
            ConsoleApi consoleApi = new ConsoleApi();
            consoleApi.StartInput();
        }

      
        static async void DisplayResult()
        {
            string result = await Funct();
            Console.WriteLine(result);
        }

        private static Task<string> Funct()
        {
            return Task.Run(() =>
            {
                SqlDataSourceEnumerator instance =
                    SqlDataSourceEnumerator.Instance;
                System.Data.DataTable table = instance.GetDataSources();

                // Display the contents of the table.
                string a = DisplayData(table);
                return a;
            });

        }

        private static string DisplayData(System.Data.DataTable table)
        {
            string s = "";

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    s += $"{col.ColumnName} = {row[col]}" + "\n";
                }
                s += "============================\n";
            }
            return s;
        }
    }
}
