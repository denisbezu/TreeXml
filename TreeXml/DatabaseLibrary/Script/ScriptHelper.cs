using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace DatabaseLibrary.Script
{
    public class ScriptHelper
    {
        public static string DataTypeMaxLength(Node columnNode)
        {
            string maxLength = columnNode.GetAttributeValue(ObjectAttribute.MaxLength);
            string precision = columnNode.GetAttributeValue(ObjectAttribute.Precision);
            string scale = columnNode.GetAttributeValue(ObjectAttribute.Scale);
            switch (columnNode.GetAttributeValue(ObjectAttribute.Type))
            {
                case "nvarchar":
                case "nchar":
                    if (maxLength == "-1")
                        return "(max)";
                    return $"({(int.Parse(maxLength) / 2)})";
                case "char":
                case "varchar":
                    if (maxLength == "-1")
                        return "max";
                    return $"({maxLength})";
                case "decimal": return $"({precision}, {scale})";
            }
            return "";
        }

        public static string AlterTable()
        {
            return "ALTER TABLE ";
        }

        public static string AddConstraint()
        {
            return "ADD CONSTRAINT ";
        }

        public static string CheckConstraint()
        {
            return "CHECK CONSTRAINT ";
        }
    }
}