using System.Text;

namespace TreeXmlLibrary
{
    public class NodeXmlString
    {
        public string DrawTree(Node rootNode)//рисуем дерево
        {
            var level = 0;
            var tempStr = new StringBuilder();
            SubNodeToString(rootNode, tempStr, level);
            return tempStr.ToString();
        }

        private void SubNodeToString(Node node, StringBuilder sb, int level)//рисуем узел
        {
            sb.Append(Repeat("\t", level));
            sb.Append("<" + node.Name);
            if (node.Attributes != null)
            {
                foreach (var attribute in node.Attributes)
                {
                    sb.Append(" " + attribute.Name + "=\"" + attribute.Value + "\"");
                }
            }
            bool hasValue = false;
            if ((node.Children == null || node.Children.Count == 0) && !string.IsNullOrEmpty(node.Value))
            {
                sb.Append(">");
                hasValue = true;
                sb.Append(node.Value);
            }
            else if (node.Children == null || node.Children.Count == 0)
            {
                sb.Append("/>\n");
                return;
            }
            else
                sb.Append(">\n");

            foreach (Node ch in node.Children)
            {
                int nextLevel = level + 1;
                SubNodeToString(ch, sb, nextLevel);
            }
            if (!hasValue)
                sb.Append(Repeat("\t", level) + "</" + node.Name + ">\n");
            else
                sb.Append("</" + node.Name + ">\n");

        }

        private string Repeat(string s, int countLevel)//добавляем отступы согласно уровню вложенности
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