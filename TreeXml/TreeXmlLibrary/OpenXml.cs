using System;
using System.IO;
using System.Xml;
using TreeXmlLibrary.interfaces;

namespace TreeXmlLibrary
{

    public class OpenXml : IOpenFile
    {

        private XmlReader _reader;

        public Tree Open(string path) // открытие файла
        {
            Tree tree = null;
            try
            {
                _reader = XmlReader.Create(path);
                while (_reader.Read())
                {
                    if (_reader.IsStartElement())
                    {
                        var rootNode = CreateNode();
                        tree = new Tree(rootNode);
                        if (!_reader.IsEmptyElement)
                            ReadNode(rootNode);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                _reader.Close();
                throw;
            }
            _reader.Close();
            return tree;
        }

        private void ReadNode(Node parent)//прочтение уровня
        {
            bool exitReader = false;
            while (_reader.Read())
            {
                switch (_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            bool isEmpty = _reader.IsEmptyElement;
                            var data = CreateNode();
                            parent.AddChild(data);
                            if (!isEmpty)
                                ReadNode(data);
                            break;
                        }
                    case XmlNodeType.Text:
                        {
                            parent.Value = _reader.Value;
                            break;
                        }
                    case XmlNodeType.EndElement:
                        exitReader = true;
                        break;
                }
                if (exitReader)
                    break;
            }
        }

        private Node CreateNode() //создание узла
        {
            var data = new Node(_reader.Name);
            if (!_reader.HasAttributes) return data;
            while (_reader.MoveToNextAttribute())
            {
                data.AddAttribute(_reader.Name, _reader.Value);
            }
            return data;
        }
    }
}