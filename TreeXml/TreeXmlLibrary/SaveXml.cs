using System;
using System.Reflection;
using System.Xml;

namespace TreeXmlLibrary
{
    public class SaveXml
    {
        public void CreateXml<T>(Node<T> root, string path) where T:class  // скорее всего переделать на дерево, но не уверен
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(root.Value.GetType().GetTypeInfo().Name + "s");
                if (root.Children != null)
                    SaveNode(writer, root);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }
        private void SaveNode<T>(XmlWriter writer, Node<T> parent) where T : class 
        {
            foreach (var child in parent.Children)
            {
                writer.WriteStartElement(parent.Value.GetType().GetTypeInfo().Name);
                foreach (var prop in child.Value.GetType().GetProperties())
                {
                    writer.WriteAttributeString(prop.Name, prop.GetValue(child.Value, null).ToString());
                }
                if (child.Children != null)
                    SaveNode(writer, child);
                writer.WriteEndElement();
            }
        }
    }
}