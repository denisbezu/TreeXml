using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TreeXmlLibrary
{
    public class Saver
    {
        private XmlReader _reader;
        //добавить проверку на имена элементов (посмотреть), не уверен, что нужна 
        public Tree<Employee> LoadXml(string path, out bool errorChecker, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                _reader = XmlReader.Create(path);
            }
            catch (FileNotFoundException e)
            {
                errorChecker = false;
                errorMessage = e.Message;
                return null;
            }
            var tree = new Tree<Employee>();
            int level = 0;
            try
            {
                while (_reader.Read())
                {
                    if (_reader.IsStartElement())
                    {
                        tree.AddRoot(new Employee() {LastName = _reader.Name});
                        level++;
                        if (_reader.IsEmptyElement)
                        {
                            //если первый пустой, добавить проверку
                        }
                        else
                        {
                            try
                            {
                                ReadNode(ref level, tree, tree.Root);
                            }
                            catch (Exception e)
                            {
                                errorChecker = false;
                                errorMessage = e.Message;
                                return null;
                            }
                        }
                    }
                    else
                        level--;
                }
            }
            catch (XmlException e)
            {
                errorChecker = false;
                errorMessage = e.Message;
                return null;
            }
            errorChecker = true;      
            return tree;
        }

        private void ReadNode(ref int level, Tree<Employee> tree, Node<Employee> parent)
        {
            int currentLvl = level;
            while (_reader.Read())
            {
                if (_reader.IsStartElement())
                {
                    level++;
                    Employee empToAdd;
                    if (_reader.IsEmptyElement)
                    {
                        empToAdd = MakeEmployee();
                        tree.AddNode(empToAdd, parent);
                    }
                    else
                    {
                        empToAdd = MakeEmployee();
                        var addedNode = tree.AddNode(empToAdd, parent);
                        if(addedNode == null)
                            throw new Exception("I cannot add an existing item to a tree : id = " + empToAdd.Id);
                        ReadNode(ref level, tree, addedNode);
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
            if (_reader.HasAttributes)
            {
                for (int i = 0; i < _reader.AttributeCount; i++)
                {
                    s.Append("  " + _reader[i]);
                }
            }

            Console.WriteLine(s);
        }
        private Employee MakeEmployee()
        {
            Employee employee = new Employee()
            {
                Name = _reader.GetAttribute("Name"),
                LastName = _reader.GetAttribute("LastName"),
                Position = _reader.GetAttribute("Position"),
                Id = int.Parse(_reader.GetAttribute("Id")),
                Age = int.Parse(_reader.GetAttribute("Age"))
            };
            return employee;
        }
    }
}