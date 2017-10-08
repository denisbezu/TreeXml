using System;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;
namespace DatabaseLibrary.Script
{
    public class TypeScript : DbItemScriptBase
    {
      public TypeScript(Node node) : base(node)
        {
            
        }

        protected override string GetScript()
        {
            string typeName = CurrentNode.GetAttributeValue(ObjectAttribute.Name);
            string typeSchema = CurrentNode.GetAttributeValue(ObjectAttribute.SchemaName);
            string typeBaseType = CurrentNode.GetAttributeValue(ObjectAttribute.Type);
            string typeNullableOption = CurrentNode.GetAttributeValue(ObjectAttribute.NullableOption) == "True" ? "NULL" : "NOT NULL";
            string typeMaxLength = ScriptHelper.DataTypeMaxLength(CurrentNode);
            if(CheckParametersForNull(typeBaseType, typeNullableOption, typeMaxLength))
            return $"CREATE TYPE [{typeSchema}].[{typeName}] FROM [{typeBaseType}]{typeMaxLength} {typeNullableOption}";
            return string.Empty;
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.UserType:
                case GroupItem.UserTypes:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }
    }
}