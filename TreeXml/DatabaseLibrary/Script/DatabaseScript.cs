using System;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class DatabaseScript : DbItemScriptBase
    {
        private string _dbName;

        public DatabaseScript(Node node) : base(node)
        {
            _dbName = node.GetAttributeValue(ObjectAttribute.Name);
        }

        protected override string GetScript()
        {
            StringBuilder dbScript = new StringBuilder();
            dbScript.Append($"CREATE DATABASE [{_dbName}]").Append(Go()).Append($"USE [{_dbName}]").Append(Go());
            StringBuilder foreignKeys = new StringBuilder();
            AddScript(dbScript, CurrentNode, GroupItem.UserTypes);
            foreach (var schema in CurrentNode.GetChildByName(GroupItem.Schemas).Children)
            {
                SchemaScript schemaScript = new SchemaScript(schema);
                dbScript.Append(schemaScript.GenerateScript());
                AddScript(dbScript, schema, GroupItem.Functions);
                var tablesNode = schema.GetChildByName(GroupItem.Tables);
                if (tablesNode != null)
                    foreach (var table in tablesNode.Children)
                    {
                        TableScript tableScript = new TableScript(table, false);
                        dbScript.Append(tableScript.GenerateScript());
                        AddScript(dbScript, table, GroupItem.Indexes);
                        AddScript(dbScript, table, GroupItem.Triggers);
                        foreignKeys.Append(tableScript.GetKey(SingleItem.ForeignKey));
                    }
                AddScript(dbScript, schema, GroupItem.Views);
                AddScript(dbScript, schema, GroupItem.Procedures);
            }
            dbScript.Append(foreignKeys);
            return dbScript.ToString();
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Database:
                case GroupItem.Databases:
                    ItemName = CurrentNode.Name;
                    break;
                default:
                    throw new ArgumentException("Invalid name");
            }
        }


        private void AddScript(StringBuilder scriptString, Node parentNode, string childName)
        {
            FactoryQuery factoryQuery = new FactoryQuery();
            var parentNodeItem = parentNode?.GetChildByName(childName);
            if (parentNodeItem == null) return;
            foreach (var child in parentNodeItem.Children)
            {
                DbItemScriptBase script = factoryQuery.CreateScriptItem(child);
                scriptString.Append(script.GenerateScript());
            }
        }
    }
}