using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class SchemaReader : DataBaseItem
    {
        public SchemaReader()
        {
            NodeName = SingleItem.Schema;
        }

        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.SchemaQuery);
        }

        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Tables);
            parentNode.AddChild(GroupItem.Views);
            parentNode.AddChild(GroupItem.Procedures);
            parentNode.AddChild(GroupItem.Functions);
        }
    }
}