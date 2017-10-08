using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ProcedureReader : DataBaseItem
    {
        public ProcedureReader()
        {
            NodeName = SingleItem.Procedure;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Format(Resources.ProcedureQuery, dbItems[0]);
        }

        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Parameters);
        }

        protected override string[] ParameterFormer(Node parentNode)
        {
            var schemaName = parentNode.FindAncestor(SingleItem.Schema).GetAttributeValue(ObjectAttribute.Name);
            return new[] { schemaName };
        }
    }
}