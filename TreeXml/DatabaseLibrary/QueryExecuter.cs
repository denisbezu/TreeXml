using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DatabaseLibrary
{
    public class QueryExecuter
    {
        public DataTable ExecuteQuery(string query, SqlConnection connection) // выполнение запроса
        {
            var dataTable = new DataTable();
            DbDataReader dataReader = null;
            try
            {
                DbCommand cmd = new SqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                dataTable.Load(dataReader);
            }
            finally
            {
                dataReader?.Close();
            }
            return dataTable;
        }
    }
}