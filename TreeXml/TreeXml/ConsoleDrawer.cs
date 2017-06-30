using System;
using System.Reflection;
using System.Text;
using TreeXmlLibrary;

namespace TreeXml
{
    public class ConsoleDrawer
    {
        public string DrawTree<T>(Node<T> rootNode) where T : class
        {
            var level = 0;
            var tempStr = new StringBuilder("<" + rootNode.Value.GetType().GetTypeInfo().Name + "s" + ">\n");
            foreach (Node<T> child in rootNode.Children)
            {
                int nextLevel = level + 1;
                SubNodeToString(child, tempStr, nextLevel);
            }
            tempStr.Append("</" + rootNode.Value.GetType().GetTypeInfo().Name + "s" + ">\n");
            return tempStr.ToString();
        }
        private void SubNodeToString<T>(Node<T> node, StringBuilder sb, int level) where T : class
        {
            sb.Append(Repeat("\t", level));
            sb.Append("<" + node.Value.GetType().GetTypeInfo().Name + " ");

            foreach (var prop in node.Value.GetType().GetProperties())
            {
                sb.Append(prop.Name + "=\"" + prop.GetValue(node.Value, null) + "\" ");
            }
            
            if (node.Children == null || node.Children.Count == 0)
            {
                sb.Append("/>\n");
                return;
            }
            else
                sb.Append(">\n");

            foreach (Node<T> ch in node.Children)
            {
                int nextLevel = level + 1;
                SubNodeToString(ch, sb, nextLevel);
            }
            sb.Append(Repeat("\t", level) + "</" + node.Value.GetType().GetTypeInfo().Name + ">\n");
        }
        private string Repeat(string s, int countLevel)
        {
            var indent = new StringBuilder();
            for (int i = 0; i < countLevel; i++)
            {
                indent.Append(s);
            }
            return indent.ToString();
        }
    }
}