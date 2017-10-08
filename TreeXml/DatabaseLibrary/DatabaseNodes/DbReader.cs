using System.Data.SqlClient;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class DbReader : DataBaseItem
    {
        public DbReader()
        {
            NodeName = SingleItem.Database;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.DatabaseQuery);
        }

        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Schemas);
            parentNode.AddChild(GroupItem.UserTypes);
        }

        private string GetQueryOneDb(string dbName)
        {
            return string.Format(Resources.OneDbQuery, dbName);
        }

        public void CreateDb(Node parentNode, SqlConnection connection, string dbName)
        {
            LoadDbItem(parentNode, GetQueryOneDb(dbName), connection);
        }


    }
}