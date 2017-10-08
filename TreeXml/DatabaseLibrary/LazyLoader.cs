using System.Data.SqlClient;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary
{
    public class LazyLoader
    {
        public void LoadNode(Node node, ConnectionData connectionData)
        {
            using (var connection = connectionData.CreateConnection())
            {
                FactoryQuery factoryQuery = new FactoryQuery();
                if (GroupItem.IsGroupItem(node.Name))
                {
                    string dbName = connectionData.GetNewInitialCatalog(node);
                    if (dbName != null && connection.Database != dbName)//
                        connection.ChangeDatabase(dbName);
                    factoryQuery.SelectDbItem(node).AddChildren(node, connection);
                }
                if (SingleItem.IsSingleItem(node.Name))
                {
                    if (node.Name.Equals(SingleItem.Server))
                        factoryQuery.SelectDbItem(node).AddEmptyNodes(node);
                    else
                        factoryQuery.SelectDbItem(node.Parent).AddEmptyNodes(node);
                }
            }
        }


    }
}