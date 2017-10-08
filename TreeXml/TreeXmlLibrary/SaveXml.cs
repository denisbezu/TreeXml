using System.Text;
using System.Xml;
using TreeXmlLibrary.interfaces;

namespace TreeXmlLibrary
{
    public class SaveXml : ISaveFile
    {
        public void Save(Node root, string path) // сохранение
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.Encoding = new UnicodeEncoding();
            settings.Indent = true;
            settings.IndentChars = "\t";
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                //writer.Formatting = Formatting.Indented;
                //writer.Indentation = 4;
               
                writer.WriteStartDocument();
                SaveNode(writer, root);
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        private void SaveNode(XmlWriter writer, Node node) // сохранение узла
        {
            writer.WriteStartElement(node.Name);
            if (node.Attributes != null)
            {
                foreach (var attribute in node.Attributes)
                {
                    writer.WriteAttributeString(attribute.Name, ReplaceChars(attribute.Value));
                }
            }
            if (!string.IsNullOrEmpty(node.Value))
                writer.WriteString(node.Value);
            if (node.Children != null)
                foreach (var child in node.Children)
                {
                    SaveNode(writer, child);
                }
            writer.WriteEndElement();
        }

        private string ReplaceChars(string value)
        {
            return value.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\'").Replace("\t", "\\t");
        }
        
    }
}