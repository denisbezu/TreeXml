using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using TreeXmlLibrary;

namespace DatabaseLibrary.DatabaseNodes
{
    public class ServerReader : DataBaseItem
    {
        public ServerReader()
        {
            NodeName = SingleItem.Server;
        }
        protected override string GetQuery(params string[] dbItems)
        {
            return string.Empty;
        }
        
        public override void AddEmptyNodes(Node parentNode)
        {
            parentNode.AddChild(GroupItem.Databases);
        }
    }
}