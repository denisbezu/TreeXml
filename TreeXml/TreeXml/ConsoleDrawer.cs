using System.Text;
using TreeXmlLibrary;

namespace TreeXml
{
    public class ConsoleDrawer
    {
        public string DrawTree<T>(Node<T> rootNode) where T:class
        {
            var level = 0;
            var tempStr = new StringBuilder(rootNode.Instance.ToString());
            foreach (Node<T> child in rootNode.Children)
            {
                int nextLevel = level + 1;
                SubNodeToString(child, tempStr, nextLevel);
            }
            return tempStr.ToString();
        }
        private void SubNodeToString<T>(Node<T> node, StringBuilder sb, int level) where T:class 
        {
            sb.Append("\n" + Repeat("\t", level));
            sb.Append(node.Instance);
            sb.Append(string.Format(" (Parent: {0})", node.Parent.Instance));
            foreach (Node<T> ch in node.Children)
            {
                int nextLevel = level + 1;
                SubNodeToString(ch, sb, nextLevel);
            }
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