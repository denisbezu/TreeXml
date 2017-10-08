using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ColumnReader : DataBaseItem
    {
        private bool _tableColumnType = true;

        public ColumnReader()
        {
            NodeName = SingleItem.Column;
        }

        protected override string GetQuery(params string[] dbItems)
        {
            return _tableColumnType ? GetTableColumn(dbItems[0], dbItems[1]) : GetViewColumn(dbItems[0], dbItems[1]);
        }

        private string GetViewColumn(string schemaName, string viewName)
        {
            return string.Format(Resources.ViewColumnQuery, viewName, schemaName);
        }

        private string GetTableColumn(string schemaName, string tableName)
        {
            return string.Format(Resources.TableColumnQuery, schemaName, tableName);
        }

        public override void AddChildren(Node parentNode, SqlConnection connection)
        {
            string singleItem = parentNode.Parent.Name;
            if (singleItem == SingleItem.View)
                _tableColumnType = false;
            base.AddChildren(parentNode, connection);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.Parent.GetAttributeValue(ObjectAttribute.Name);
            return new[] {schemaName, tableName};
        }
    }
}