using System;
using System.Collections.ObjectModel;
using System.Linq;
using DatabaseLibrary.Enums;
using TreeWPF.ViewModels;
using TreeXmlLibrary;

namespace TreeWPF.Helpers.SaveOpen
{
    public class OpenNodes
    {
        #region ctor

        public Tree OpenFile(string filename)
        {
            OpenXml openXml = new OpenXml();
            return openXml.Open(filename);
        }

        #endregion

        /// <summary>
        /// Метод для создания дерева сервера
        /// </summary>
        /// <param name="onSelectedAction">делегат для отслеживания IsSelectedChanged</param>
        /// <param name="tree">дерево, полученное из xml</param>
        /// <returns></returns>
        public NodeViewModel MakeTreeViewModel(Action<object> onSelectedAction, Tree tree)
        {
            var xmlRoot = tree.Root; //коренной элемент из xml
            var rootNode = MakeDatabaseNode(xmlRoot); // по сути Node, который пойдет потом в NodeViewModel
            var rootViewModel = MakeDatabaseViewModel(xmlRoot, onSelectedAction, rootNode); //nodeViewModel
            MakeNewDatabaseNode(onSelectedAction, xmlRoot, rootViewModel, rootNode);
            return rootViewModel;
        }
        /// <summary>
        /// Создание нового узла в дереве
        /// </summary>
        /// <param name="onSelectedAction">делегат для отслеживания IsSelectedChanged</param>
        /// <param name="parent">родительский элемент полученный из xml, т.е. с атрибутами isExpanded, isSelected и т.д.</param>
        /// <param name="parentViewModel">viewModel родительского элемента</param>
        /// <param name="parentNode">родительский элемент самого узла, т.е. Node, который хранится во viewModel</param>
        private void MakeNewDatabaseNode(Action<object> onSelectedAction, Node parent, NodeViewModel parentViewModel, Node parentNode)
        {
            foreach (var child in parent.Children)
            {
                var childNode = MakeDatabaseNode(child); // дочерний элемент самого узла, т.е. Node, который хранится во viewModel
                parentNode.AddChild(childNode); // добавляем его к родителю
                var childViewModel = MakeDatabaseViewModel(child, onSelectedAction, childNode); // дочерняя viewModel
                parentViewModel.Children.Add(childViewModel); // добавляем ее к родительской
                if (child.Children.Count != 0)// рекурсивно вызываем функцию для добавления всех дочерних элементов
                    MakeNewDatabaseNode(onSelectedAction, child, childViewModel, childNode);
                else
                {//добавляем пустой элемент, если узел не раскрыт
                    if (!childViewModel.IsExpanded && childViewModel.CanExpand())
                        childViewModel.Children.Add(GetDummyChild());
                }
            }
        }

        private NodeViewModel MakeDatabaseViewModel(Node node, Action<object> onSelectedAction, Node currentNode)
        {
            var nodeViewModelMomento = new NodeViewModelMomento(node.GetAttributeValue("NodeName"),
                 currentNode, node.Name, node.GetAttributeValue("IsSelected") == "True",
                 node.GetAttributeValue("IsExpanded") == "True", onSelectedAction);
            NodeViewModel viewModel = new NodeViewModel(nodeViewModelMomento);
            return viewModel;
        }
        /// <summary>
        /// Создание узла для NodeViewModel
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node MakeDatabaseNode(Node node)
        {
            Node currentNode = new Node(node.GetAttributeValue("Node.MainName"));
            foreach (var attribute in node.Attributes)
                if (attribute.Name.StartsWith("Node.") && attribute.Name != "Node.MainName")
                {
                    currentNode.AddAttribute(attribute.Name.Substring(5), attribute.Value);
                }
            return currentNode;
        }
       
        /// <summary>
        /// Создание пустого узла
        /// </summary>
        /// <returns></returns>
        private NodeViewModel GetDummyChild()
        {
            var nodeViewModelMomento = new NodeViewModelMomento("", new Node(), "", false, false);
            NodeViewModel viewModel = new NodeViewModel(nodeViewModelMomento);
            return viewModel;
        }
       }
}