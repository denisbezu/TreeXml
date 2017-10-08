using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class TriggerReader : DataBaseItem
    {
        public TriggerReader()
        {
            NodeName = SingleItem.Trigger;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.TriggerQuery, dbItems[0], dbItems[1]);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.FindAncestor(SingleItem.Table).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName, tableName };
        }
    }
}