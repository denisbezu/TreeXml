using System;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class SchemaScript : DbItemScriptBase
    {
        public SchemaScript(Node node) : base(node)
        {

        }

        protected override string GetScript()
        {
            string schemaName = CurrentNode.GetAttributeValue(ObjectAttribute.Name);
            if (CheckParametersForNull(schemaName))
                return $"CREATE SCHEMA [{schemaName}]";
            return String.Empty;
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Schema:
                    ItemName = SingleItem.Schema;
                    break;
                case GroupItem.Schemas:
                    ItemName = GroupItem.Schemas;
                    break;
                default: throw new Exception("Invalid name");
            }
        }
    }
}