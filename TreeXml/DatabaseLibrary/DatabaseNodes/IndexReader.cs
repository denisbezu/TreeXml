using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class IndexReader : DataBaseItem
    {
        public IndexReader()
        {
            NodeName = SingleItem.Index;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.IndexQuery, dbItems[0], dbItems[1]);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            var tableName = parentNode.FindAncestor(SingleItem.Table).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName, tableName };
        }
    }
}