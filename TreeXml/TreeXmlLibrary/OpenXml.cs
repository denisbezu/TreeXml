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
                                _reader.Close();
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
                _reader.Close();
                return null;
            }
            errorChecker = true;
            _reader.Close();
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
                            throw new Exception("You are trying to add an existing item to the tree, please, change your tree");
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
        private T MakeInstance<T>() where T : class
        {
            T currentType = Activator.CreateInstance<T>();
            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {
                    PropertyInfo propertyInfo = currentType.GetType().GetProperty(_reader.Name);
                    if (propertyInfo != null && (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?)))
                    {
                       propertyInfo.SetValue(currentType, int.Parse(_reader.Value), null);
                    }
                    else if (propertyInfo != null && propertyInfo.PropertyType == typeof(double))
                    {
                        propertyInfo.SetValue(currentType, double.Parse(_reader.Value), null);
                    }
                    else if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(currentType, bool.Parse(_reader.Value), null);
                    }
                    else
                    {
                        if (propertyInfo != null)
                            propertyInfo.SetValue(currentType, _reader.Value, null);
                    }

                }
            }
            return currentType;
        }
    }
}