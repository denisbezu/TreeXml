using System;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class KeyScript : DbItemScriptBase
    {
        private string _keyName;
        private string _tableName;
        private string _schemaName;

        public KeyScript(Node node) : base(node)
        {
            _keyName = node.GetAttributeValue(ObjectAttribute.Name);
            _tableName = node.FindAncestor(SingleItem.Table)?.GetAttributeValue(ObjectAttribute.Name);
            _schemaName = node.FindAncestor(SingleItem.Schema)?.GetAttributeValue(ObjectAttribute.Name);
        }

        protected override string GetScript()
        {
            string constraintType = CurrentNode.Name;
            switch (constraintType)
            {
                case SingleItem.PrimaryKey:
                    return GetPrimaryKeyScript();
                case SingleItem.ForeignKey:
                    return GetForeignKeyScript();
                default: throw new ArgumentOutOfRangeException(@"Invalid key name");
            }
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.PrimaryKey:
                case SingleItem.ForeignKey:
                case GroupItem.Keys:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }

        private string GetForeignKeyScript()
        {
            string referencedSchema = CurrentNode.GetAttributeValue(ObjectAttribute.ReferencedSchema);
            string fkColumn = CurrentNode.GetAttributeValue(ObjectAttribute.ColumnName).Trim().Replace(",", "],[");
            string refColumn = CurrentNode.GetAttributeValue(ObjectAttribute.ReferenceColumnName).Trim().Replace(",", "],[");

            string refTable = CurrentNode.GetAttributeValue(ObjectAttribute.ReferenceTableName);
            string noCheckOption = CurrentNode.GetAttributeValue(ObjectAttribute.NoCheckOption) == "False" ? "CHECK" : "NOCHECK";
            if (CheckParametersForNull(referencedSchema, fkColumn, refColumn, refTable, noCheckOption))
                return
                    $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] WITH {noCheckOption} {ScriptHelper.AddConstraint()}[{_keyName}]" +
                    $" FOREIGN KEY ([{fkColumn}])\nREFERENCES [{referencedSchema}].[{refTable}] ([{refColumn}])" + Go() +
                    $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] {ScriptHelper.CheckConstraint()}[{_keyName}]";
            return string.Empty;
        }

        private string GetPrimaryKeyScript()
        {
            string[] keysFound = CurrentNode.GetAttributeValue(ObjectAttribute.ColumnName).Trim().Split(',');
            string type = CurrentNode.GetAttributeValue(ObjectAttribute.Type);
            StringBuilder keys = new StringBuilder();
            keys.Append($"\t[{keysFound[0]}] ASC");
            if (keysFound.Length > 1)
            {
                for (int i = 1; i < keysFound.Length; i++)
                {
                    keys.Append($",\n\t[{keysFound[i]}] ASC");
                }
            }
            if (CheckParametersForNull(type, keys.ToString()))
                return $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] " +
                       $"{ScriptHelper.AddConstraint()}[{_keyName}] PRIMARY KEY {type}\n(\n{keys}\n)";
            return string.Empty;
        }
    }
}