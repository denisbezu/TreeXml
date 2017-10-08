using System.Linq;

namespace DatabaseLibrary.Enums
{
    public class GroupItem
    {
        public const string Databases = "Databases";
        public const string Schemas = "Schemas";
        public const string UserTypes = "UserTypes";
        public const string Tables = "Tables";
        public const string Views = "Views";
        public const string Columns = "Columns";
        public const string Keys = "Keys";
        public const string Constraints = "Constraints";
        public const string Procedures = "Procedures";
        public const string Triggers = "Triggers";
        public const string Indexes = "Indexes";
        public const string Parameters = "Parameters";
        public const string Functions = "Functions";

        public static bool IsGroupItem(string item)
        {
            return new[]
            {
                Databases, Schemas, UserTypes, Tables, Views,
                Columns, Keys, Constraints, Procedures, Triggers,
                Indexes, Parameters, Functions
            }.Any(it => it == item);
        }
    }
}