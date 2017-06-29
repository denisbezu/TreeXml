using System;
using System.CodeDom;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace TreeXmlLibrary
{
    public class OpenXml
    {
        private XmlReader _reader;
        //добавить проверку на имена элементов (посмотреть), не уверен, что нужна эта проверка
        public Tree<T> LoadXml<T>(string path, out bool errorChecker, out string errorMessage) where T : class
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
            var tree = new Tree<T>();
            int level = 0;
            try
            {
                while (_reader.Read())
                {
                    if (_reader.IsStartElement())
                    {
                        Lazy<T> root = new Lazy<T>();
                        tree.AddRoot(root.Value);
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
        private void ReadNode<T>(ref int level, Tree<T> tree, Node<T> parent) where T : class
        {
            int currentLvl = level;
            while (_reader.Read())
            {
                if (_reader.IsStartElement())
                {
                    level++;
                    T empToAdd;
                    if (_reader.IsEmptyElement)
                    {
                        empToAdd = MakeInstance<T>();
                        tree.AddNode(empToAdd, parent);
                    }
                    else
                    {
                        empToAdd = MakeInstance<T>();
                        var addedNode = tree.AddNode(empToAdd, parent);
                        if (addedNode == null)
                            throw new Exception("You are trying to add an existing item to the read, please, change your tree");
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
        private T MakeInstance<T>() where T : class
        {
            Lazy<T> currentType = new Lazy<T>();
            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {
                    PropertyInfo propertyInfo = currentType.Value.GetType().GetProperty(_reader.Name);
                    if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
                    {
                       propertyInfo.SetValue(currentType.Value, int.Parse(_reader.Value), null);
                    }
                    else if (propertyInfo != null && propertyInfo.PropertyType == typeof(double))
                    {
                        propertyInfo.SetValue(currentType.Value, double.Parse(_reader.Value), null);
                    }
                    else if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(currentType.Value, bool.Parse(_reader.Value), null);
                    }
                    else
                    {
                        if (propertyInfo != null)
                            propertyInfo.SetValue(currentType.Value, _reader.Value, null);
                    }

                }
            }
            return currentType.Value;
        }
    }
}