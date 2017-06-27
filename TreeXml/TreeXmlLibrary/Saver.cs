using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TreeXmlLibrary
{
    public class Saver
    {
        XmlReader reader;

        //добавить проверку на имена элементов 
        // добавить в out параметр, была ли ошибка какая-то
        public Node<Employee> LoadXml(string path)
        {
            reader = XmlReader.Create(path);
            Tree<Employee> tree = new Tree<Employee>();
            Employee empToAdd;
            Node<Employee> root = null;
            int level = 0;
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    root = tree.AddRoot(new Employee() { LastName = reader.Name });
                    level++;
                    if (reader.IsEmptyElement)
                    {
                        //если первый пустой, добавить проверку
                    }
                    else
                        ReadNode(reader, ref level, tree, root);
                }
                else
                    level--;
            }
            return root;
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
                    {
                        empToAdd = MakeEmployee();
                        var addedNode = tree.AddNode(empToAdd, parent);
                    }
                    else
                    {
                        empToAdd = MakeEmployee();
                        var addedNode = tree.AddNode(empToAdd, parent);
                        ReadNode(reader, ref level, tree, addedNode);
                    }
                }
                else
                {
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

        private void PrintIndent(int level, string str, bool isRoot)
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
        }

        private Employee MakeEmployee()
        {
            Employee employee = new Employee()
            {
                Name = reader.GetAttribute("Name"),
                LastName = reader.GetAttribute("LastName"),
                Position = reader.GetAttribute("Position"),
                Id = int.Parse(reader.GetAttribute("Id")),
                Age = int.Parse(reader.GetAttribute("Age"))
            };
            return employee;
        }
    }
}