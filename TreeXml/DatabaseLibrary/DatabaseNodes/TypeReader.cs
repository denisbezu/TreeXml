using DatabaseLibrary.Enums;
using DatabaseLibrary.Interfaces;
using DatabaseLibrary.Properties;

namespace DatabaseLibrary.DatabaseNodes
{
    public class TypeReader : DataBaseItem
    {
        public TypeReader()
        {
            NodeName = SingleItem.UserType;
        }
        protected override string GetQuery(params string [] dbItems)
        {
            return Resources.TypeQuery;
        }
    }
}