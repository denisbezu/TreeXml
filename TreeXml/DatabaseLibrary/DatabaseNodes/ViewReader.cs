using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ViewReader : DataBaseItem
    {
        public ViewReader()
        {
            NodeName = SingleItem.View;
        }

        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.ViewQuery, dbItems[0]);
        }

        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Columns);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName };
        }
    }
}