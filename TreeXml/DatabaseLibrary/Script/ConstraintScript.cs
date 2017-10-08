using System;
using System.Diagnostics;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class ConstraintScript : DbItemScriptBase
    {
        private string _constraintName;
        private string _tableName;
        private string _schemaName;

        public ConstraintScript(Node node) : base(node)
        {
            _tableName = node.FindAncestor(SingleItem.Table)?.GetAttributeValue(ObjectAttribute.Name);
            _schemaName = node.FindAncestor(SingleItem.Schema)?.GetAttributeValue(ObjectAttribute.Name);
            _constraintName = node.GetAttributeValue(ObjectAttribute.Name);
        }

        protected override string GetScript()
        {
            var constraintType = CurrentNode.Name;
            switch (constraintType)
            {
                case SingleItem.CheckConstraint:
                    return GetCheckConstraintScript();
                case SingleItem.DefaultConstraint:
                    return GetDefaultConstraintScript();
                default:
                    throw new ArgumentOutOfRangeException("Invalid constraint name");
            }
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.DefaultConstraint:
                case SingleItem.CheckConstraint:
                case GroupItem.Constraints:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }

        private string GetDefaultConstraintScript()
        {
            string defValue = CurrentNode.GetAttributeValue(ObjectAttribute.DefaultValue);
            string forColumn = CurrentNode.GetAttributeValue(ObjectAttribute.ColumnName);
            return CheckParametersForNull(defValue, forColumn) ? $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] {ScriptHelper.AddConstraint()}[{_constraintName}] DEFAULT {defValue} FOR [{forColumn}]" : string.Empty;
        }

        private string GetCheckConstraintScript()
        {
            string checkDefinition = CurrentNode.GetAttributeValue(ObjectAttribute.Definition);
            string noCheckOption = CurrentNode.GetAttributeValue(ObjectAttribute.NoCheckOption) == "False" ? "CHECK" : "NOCHECK";
            if (CheckParametersForNull(checkDefinition, noCheckOption))
                return
                    $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] WITH {noCheckOption} {ScriptHelper.AddConstraint()}[{_constraintName}] " +
                    $"CHECK ({checkDefinition})" + Go() +
                    $"{ScriptHelper.AlterTable()}[{_schemaName}].[{_tableName}] {ScriptHelper.CheckConstraint()}[{_constraintName}]";
            return string.Empty;
        }


    }
}