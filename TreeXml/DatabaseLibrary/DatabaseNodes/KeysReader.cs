using System.Data.SqlClient;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class KeysReader : DataBaseItem
    {
        public KeysReader()
        {
            NodeName = SingleItem.PrimaryKey;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            //PRIMARY KEY
            return string.Format(Resources.PrimaryKeyQuery, dbItems[0], dbItems[1]);
        }

        private string GetForeignKeys(params string[] dbItems)
        {
            return string.Format(Resources.ForeignKeyQuery, dbItems[0], dbItems[1]);
        }

        public override void AddChildren(Node parentNode, SqlConnection connection)
        {
            base.AddChildren(parentNode, connection);
            NodeName = SingleItem.ForeignKey;
            LoadDbItem(parentNode, GetForeignKeys(ParameterFormer(parentNode)), connection);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.FindAncestor(SingleItem.Table).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName, tableName };
        }
    }
}