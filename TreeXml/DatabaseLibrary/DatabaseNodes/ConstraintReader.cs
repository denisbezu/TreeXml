using System.Data.SqlClient;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ConstraintReader : DataBaseItem
    {
        public ConstraintReader()
        {
            NodeName = SingleItem.DefaultConstraint;
        }

        protected override string GetQuery(params string[] dbItems)
        {
            //DEFAULT CONSTRAINTS
            return string.Format(Resources.DefaultConstraintQuery, dbItems[1], dbItems[0]);
        }

        public override void AddChildren(Node parentNode, SqlConnection connection)
        {
            base.AddChildren(parentNode, connection);
            NodeName = SingleItem.CheckConstraint;
            LoadDbItem(parentNode, GetCheckConstraints(ParameterFormer(parentNode)), connection);
        }

        private string GetCheckConstraints(params string[] dbItems)
        {
            return string.Format(Resources.CheckConstraintQuery, dbItems[1], dbItems[0]);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.FindAncestor(SingleItem.Table).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName, tableName };
        }
    }
}