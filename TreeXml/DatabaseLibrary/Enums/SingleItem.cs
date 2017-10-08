using System.Linq;

namespace DatabaseLibrary.Enums
{
    public class SingleItem
    {
        public const string Database = "Database";
        public const string Schema = "Schema";
        public const string UserType = "UserType";
        public const string DefaultConstraint = "DefaultConstraint";
        public const string Index = "Index";
        public const string CheckConstraint = "CheckConstraint";
        public const string PrimaryKey = "PrimaryKey"; 
        public const string ForeignKey = "ForeignKey";
        public const string View = "View";
        public const string Column = "Column";
        public const string Trigger = "Trigger";
        public const string Procedure = "Procerdure";
        public const string Parameter = "Parameter";
        public const string Table = "Table";
        public const string Function = "Function";
        public const string Server = "Server";
        public static bool IsSingleItem(string item)
        {
            return new[]
            {
                Database, Schema, UserType, Table, View,
                Column, PrimaryKey, ForeignKey, DefaultConstraint, CheckConstraint,
                Procedure, Trigger, Index, Parameter, Function, Server
            }.Any(it => it == item);
        }
    }
}