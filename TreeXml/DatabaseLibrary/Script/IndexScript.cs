using System;
using System.Text;
using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class IndexScript : DbItemScriptBase
    {

        public IndexScript(Node node) : base(node)
        { }

        protected override string GetScript()
        {
            string tableName = CurrentNode.FindAncestor(SingleItem.Table)?.GetAttributeValue(ObjectAttribute.Name);
            string schemaName = CurrentNode.FindAncestor(SingleItem.Schema)?.GetAttributeValue(ObjectAttribute.Name);
            string indexName = CurrentNode.GetAttributeValue(ObjectAttribute.Name);
            string[] indexesFound = CurrentNode.GetAttributeValue(ObjectAttribute.ColumnName).Trim().Split(',');
            string isUnique = bool.Parse(CurrentNode.GetAttributeValue(ObjectAttribute.IsUnique)) ? "UNIQUE" : "";
            var indexType = CurrentNode.GetAttributeValue(ObjectAttribute.Type);
            StringBuilder indexes = new StringBuilder();
            indexes.Append($"\t[{indexesFound[0]}] ASC");
            if (indexesFound.Length > 1)
                for (var i = 1; i < indexesFound.Length; i++)
                    indexes.Append($",\n\t[{indexesFound[i]}] ASC");
            if (indexType == "XML")
                return "";
            if (CheckParametersForNull(isUnique, indexType))
                return $"CREATE {isUnique} {indexType} INDEX [{indexName}]" +
                       $" ON [{schemaName}].[{tableName}] \n(\n{indexes}\n)";
            return string.Empty;
        }

        protected override void SetNames()
        {
            switch (CurrentNode.Name)
            {
                case SingleItem.Index:
                case GroupItem.Indexes:
                    ItemName = CurrentNode.Name;
                    break;
                default: throw new ArgumentException("Invalid name");
            }
        }
    }
}