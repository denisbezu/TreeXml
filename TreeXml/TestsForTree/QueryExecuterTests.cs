using System.Data;
using System.Data.SqlClient;
using DatabaseLibrary;
using DatabaseLibrary.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestsForTree
{
    [TestClass()]
    public class QueryExecuterTests
    {
        [TestMethod()]
        public void ExecuteQueryTest()
        {
            ConnectionData connectionData = new ConnectionData();
            connectionData.CreateConnectionString("default");
            QueryExecuter executer = new QueryExecuter();
            DataTable result = null;
            using (var connection = connectionData.CreateConnection())
            {
                result = executer.ExecuteQuery(Resources.DatabaseQuery, connection);
            }
            Assert.IsNotNull(result);
        }
    }
}