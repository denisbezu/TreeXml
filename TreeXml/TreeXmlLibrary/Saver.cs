using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TreeXmlLibrary
{
    public class Saver
    {
        XmlReader reader = XmlReader.Create("denys.xml");
        public Tree<Employee> LoadXml()
        {
            Tree<Employee> tree = new Tree<Employee>();
            Employee empToAdd;
            Node<Employee> root;
            int level = 0;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    root = tree.AddRoot(new Employee() {LastName = reader.Name});
                    level++;
                    if (reader.IsEmptyElement)
                    {
                        empToAdd = PrintIndent(level - 1, "<" + reader.Name + ">", true);
                    }
                    else
                    {
                        empToAdd = PrintIndent(level - 1, "<" + reader.Name + ">", true);
                        ReadNode(reader, ref level, tree, root);
                        //Console.WriteLine(reader.ReadString()); //Read the text content of the element.
                    }
                }
                else
                {
                    empToAdd = PrintIndent(level - 1, "</" + reader.Name + ">", true);
                    level--;
                }
                //Console.WriteLine(level);
            }
            return tree;
        }

        private void ReadNode(XmlReader reader, ref int level, Tree<Employee> tree, Node<Employee> parent)
        {
            Employee empToAdd;
            int currentLvl = level;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    level++;
                    if (reader.IsEmptyElement)
                        empToAdd = PrintIndent(level - 1, "<" + reader.Name + ">", false);
                    else
                    {

                        empToAdd = PrintIndent(level - 1, "<" + reader.Name + ">", false);
                        var addedNode = tree.AddNode(empToAdd, parent);
                        ReadNode(reader, ref level, tree, addedNode);
                        //Console.WriteLine(reader.ReadString()); //Read the text content of the element.
                    }
                }
                else
                {
                    empToAdd = PrintIndent(level - 1, "</" + reader.Name + ">", true);
                    if (currentLvl == level)
                    {
                        level--;
                        break;
                    }
                    level--;
                    if (currentLvl == level)
                    {
                        level--;
                        break;
                    }
                }
            }
        }

        private Employee PrintIndent(int level, string str, bool isRoot)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < level; i++)
                s.Append("\t");
            s.Append(str);
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    s.Append("  " + reader[i]);
                }
            }

            Console.WriteLine(s);
            Employee employee = null;
            if (!isRoot)
            {
                employee = new Employee()
                {
                    Name = reader.GetAttribute("Name"),
                    LastName = reader.GetAttribute("LastName"),
                    Position = reader.GetAttribute("Position"),
                    Id = int.Parse(reader.GetAttribute("Id")),
                    Age = int.Parse(reader.GetAttribute("Age"))
                };
            }
            return employee;
        }
    }
}