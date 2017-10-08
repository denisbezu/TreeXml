using System;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;
namespace DatabaseLibrary.Script
{
    public class TriggerScript : DbItemScriptBase
    {
        public TriggerScript(Node node) : base(node)
        {

        }


        protected override string GetScript()
        {
            string tableName = CurrentNode.FindAncestor(SingleItem.Table)?.GetAttributeValue(ObjectAttribute.Name);
            string schemaName = CurrentNode.FindAncestor(SingleItem.Schema)?.GetAttributeValue(ObjectAttribute.Name);
            string triggerName = CurrentNode.GetAttributeValue(ObjectAttribute.Name);
            string triggerDefinition = CurrentNode.GetAttributeValue(ObjectAttribute.Definition);
            if (CheckParametersForNull(triggerDefinition))
                return $"{triggerDefinition}" + Go() + $"{ScriptHelper.AlterTable()}[{schemaName}].[{tableName}] ENABLE TRIGGER [{triggerName}]";
            return string.Empty;
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Trigger:
                case GroupItem.Triggers:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }
    }
}