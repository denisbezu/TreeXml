using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class TableReader : DataBaseItem
    {
        public TableReader()
        {
            NodeName = SingleItem.Table;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.TableQuery, dbItems[0]);
        }

        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Columns);
            parentNode.AddChild(GroupItem.Keys);
            parentNode.AddChild(GroupItem.Constraints);
            parentNode.AddChild(GroupItem.Indexes);
            parentNode.AddChild(GroupItem.Triggers);
        }
        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName };
        }
    }
}