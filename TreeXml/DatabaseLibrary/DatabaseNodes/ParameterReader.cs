using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ParameterReader : DataBaseItem
    {
        public ParameterReader()
        {
            NodeName = SingleItem.Parameter;
        }

        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.ParameterQuery, dbItems[0], dbItems[1]);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.Parent.GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName, tableName };
        }
    }
}