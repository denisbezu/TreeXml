using System;
using System.Collections.Generic;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class TableScript : DbItemScriptBase
    {
        private bool _onlyTableScript;

        public TableScript(Node node, bool onlyTable = true) : base(node)
        {
            _onlyTableScript = onlyTable;
        }

        protected override string GetScript()
        {
            string schemaName = CurrentNode.FindAncestor(SingleItem.Schema)?.GetAttributeValue(ObjectAttribute.Name);
            string tableName = CurrentNode.GetAttributeValue(ObjectAttribute.Name);
            string tableColumns = GetColumns();
            StringBuilder tableScript = new StringBuilder();
            if (tableColumns != null)
                tableScript.Append($"CREATE TABLE [{schemaName}].[{tableName}] \n(\n{tableColumns}\n)").Append(Go());
            string constraints = GetConstraints();
            if (constraints != null)
                tableScript.Append(constraints);
            string primaryKey = GetKey(SingleItem.PrimaryKey);
            if (primaryKey != null)
                tableScript.Append(primaryKey);
            if (_onlyTableScript)
            {
                string foreignKey = GetKey(SingleItem.ForeignKey);
                if (foreignKey != null)
                    tableScript.Append(foreignKey);
            }
            return tableScript.ToString();
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Table:
                case GroupItem.Tables:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }

        public string GetKey(string keyItem)
        {
            if (keyItem != SingleItem.ForeignKey && keyItem != SingleItem.PrimaryKey)
                throw new Exception($"Key cannot be generated, because '{keyItem}' is not a key");
            var keysBuilder = new StringBuilder();
            var keysNode = CurrentNode.GetChildByName(GroupItem.Keys);
            if (keysNode == null)
                return null;
            foreach (var key in keysNode.Children)
            {
                var keyName = key.Name;
                if (keyName.Equals(keyItem))
                {
                    AddScript(keysBuilder, key);
                }
            }
            return keysBuilder.ToString();
        }

        private string GetConstraints()
        {
            var constraints = new StringBuilder();
            var constraintsNode = CurrentNode.GetChildByName(GroupItem.Constraints);
            if (constraintsNode == null)
                return null;
            foreach (var constraint in constraintsNode.Children)
            {
                AddScript(constraints, constraint);
            }
            return constraints.ToString();
        }

        private void AddScript(StringBuilder scriptString, Node node)
        {
            FactoryQuery factoryQuery = new FactoryQuery();
            DbItemScriptBase scriptItem = factoryQuery.CreateScriptItem(node);
            scriptString.Append(scriptItem.GenerateScript());
        }

        private string GetColumns()
        {
            Node columns = CurrentNode.GetChildByName(GroupItem.Columns);
            if (columns == null)
                return null;
            List<string> columnsFound = new List<string>();
            foreach (var child in columns.Children)
            {
                columnsFound.Add(GetColumn(child));
            }
            if (columnsFound.Count == 0)
                return string.Empty;
            StringBuilder columnsScript = new StringBuilder();
            columnsScript.Append($"\t{columnsFound[0]}");
            if (columnsFound.Count > 1)
            {
                for (int i = 1; i < columnsFound.Count; i++)
                {
                    columnsScript.Append($",\n\t{columnsFound[i]}");
                }
            }
            return columnsScript.ToString();
        }

        private string GetColumn(Node columnNode)
        {
            string name = columnNode.GetAttributeValue(ObjectAttribute.Name);
            string type = columnNode.GetAttributeValue(ObjectAttribute.Type);
            string identityValue = bool.Parse(columnNode.GetAttributeValue(ObjectAttribute.IsIdentity))
                ? $"IDENTITY({columnNode.GetAttributeValue(ObjectAttribute.SeedValue)},{columnNode.GetAttributeValue(ObjectAttribute.IncrementValue)}) "
                : "";
            string rowguid = bool.Parse(columnNode.GetAttributeValue(ObjectAttribute.IsRowguidCol)) ? "ROWGUIDCOL " : "";
            string nullableOption = bool.Parse(columnNode.GetAttributeValue(ObjectAttribute.NullableOption)) ? "NULL" : "NOT NULL";
            string computedValue = columnNode.GetAttributeValue(ObjectAttribute.ComputedDefinition) == null ?
                null : columnNode.GetAttributeValue(ObjectAttribute.ComputedDefinition);
            string columnType = columnNode.GetAttributeValue(ObjectAttribute.SchemaName) != "sys"
                ? $"[{columnNode.GetAttributeValue(ObjectAttribute.SchemaName)}]."
                : "";
            string maxLength = ScriptHelper.DataTypeMaxLength(columnNode);

            if (computedValue != null)
                return $"[{name}] AS {computedValue}";
            return $"[{name}] {columnType}[{type}]{maxLength} {identityValue}{rowguid}{nullableOption}";
        }

    }
}