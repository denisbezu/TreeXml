using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TreeXmlLibrary
{
    public class Node : ICloneable, IEnumerable
    {
        #region Fields

        private readonly Lazy<List<Node>> _children;// дочерние элементы

        private Node _parent; // родитель

        private const string DefaultName = "Undefined"; // имя узла по умолчанию

        #endregion

        #region Properties

        public Tree Tree { get; set; } // дерево

        public string Name { get; set; } // имя узла 

        public string Value { get; set; } //значение узла, если есть

        public List<Attribute> Attributes { get; private set; } // атрибуты узла

        public Node Parent // родитель
        {
            get => _parent;
            private set => _parent = value;
        }

        public List<Node> Children//дочеерние элементы
        {
            get { return _children.Value; }
        }

        #endregion

        #region Ctor

        public Node(string name)
        {
            _children = new Lazy<List<Node>>();
            Name = name;
        }

        public Node() : this(DefaultName)
        { }

        #endregion

        public string GetAttributeValue(string name)// есть ли заданный аттрибут в списке
        {
            return Attributes?.Find(att => att.Name.ToLower().Equals(name.ToLower()))?.Value;
        }

        #region AddOperations

        public void AddAttribute(string name, string value)//добавление атрибута
        {
            if (Attributes == null)
                Attributes = new List<Attribute>();
            var attribute = new Attribute(name, value);
            Attributes.Add(attribute);
        }

        public Node AddChild(string name)//добавление дочернего элемента, создавая новый узел по имени
        {
            var node = new Node(name);
            return AddChild(node);
        }

        public Node AddChild(Node child)//добавление дочернего узла, который уже существует
        {
            SetTree(child);
            child.SetParent(this);
            return child;
        }

        #endregion

        #region ParentOperations

        private void SetParent(Node parent) //задание родителя
        {
            if (parent != null)
            {
                if (_parent == null)
                {
                    _parent = parent;
                    _parent.Children.Add(this);
                }
                else
                    throw new Exception("To set a new parent, you need to remove it first");
            }
            else
                throw new Exception("Parent cannot be null. Use the remove parent method");
        }

        public void RemoveParent() // удаление родителя
        {
            SearchNodeAction(this, NodesRemoveAction());
            Tree = null;
            RemoveFromParent(this);
        }

        private void RemoveFromParent(Node child) // удаление текущего узла из списка дочерних элементов родителя
        {
            if (_parent == null)
                throw new Exception("This node hasnt a parent");
            _parent.Children.Remove(child);
            _parent = null;
        }

        #endregion

        #region Actions
        private Action<Node> NodesRemoveAction()//удаление из Nodes
        {
            return (currentNode) =>
            {
                Tree?.Nodes.Remove(currentNode);
                if (currentNode != this)
                    currentNode.Tree = null;
            };
        }

        private Action<Node> ReferenceEqualsAction(Node secondCurrent)//проверка на соответствие имеющегося узла с текущим
        {
            return (currentNode) =>
            {
                if (ReferenceEquals(secondCurrent, currentNode))
                    throw new Exception("This node exists in the tree!");
            };
        }

        private Action<Node> NodesAddAction()//добавление в Nodes
        {
            return currentNode =>
            {
                if (Tree.Nodes.Contains(currentNode))
                    throw new Exception("This node exists in the tree!");
                currentNode.Tree = Tree;
                Tree.Nodes.Add(currentNode);
            };

        }

        private void SearchNodeAction(Node root, Action<Node> check = null)// проход по дереву + действие
        {
            //var nodesStack = new Stack<Node>();
            //nodesStack.Push(root);
            //while (nodesStack.Count != 0)
            //{
            //    var currentNode = nodesStack.Pop();
            //    check?.Invoke(currentNode);
            //    foreach (var children in currentNode.Children)
            //        nodesStack.Push(children);
            //}

            foreach (var node in root)
            {
                check?.Invoke(node);
            }
        }

        #endregion

        private void SetTree(Node child)// установка значение дерева для узла
        {
            //если у добавляеемого элемента дерево не задано, а у родителя - задано
            if (child.Tree == null && Tree != null)
                SearchNodeAction(child, NodesAddAction());
            else if (child.Tree != null && child.Tree != Tree) //если при добавлении в дерево ссылки на дерево разные
                throw new Exception("This node has already a tree, remove it first");
            else if (child.Tree == null && this.Tree == null)//если и у добавлемого узла и у принимающего узла нету ссылки на дерево
            {
                //var nodesStack = new Stack<Node>();
                //nodesStack.Push(FindRoot());
                //while (nodesStack.Count != 0)
                //{
                //    var currentNode = nodesStack.Pop();
                //    SearchNodeAction(child.FindRoot(), ReferenceEqualsAction(currentNode));
                //    foreach (var children in currentNode.Children)
                //        nodesStack.Push(children);
                //}
                foreach (var node in FindRoot())
                {
                    SearchNodeAction(child.FindRoot(), ReferenceEqualsAction(node));
                }

            }
            else if (child.Tree == Tree)//если уже есть ссылка на дерево
                throw new Exception("This node is already in the tree");
        }

        public Node FindRoot()// поиск корневого элемента, если не задано дерево
        {
            Node currentNode = this;
            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
            }
            return currentNode;
        }

        public Node FindAncestor(string name) //найти предка по имени
        {
            Node currentNode = this;
            while (currentNode.Name != name)
            {
                currentNode = currentNode.Parent;
                if (currentNode == null)
                    return null;
                //throw new Exception("The ancestor with name " + name + " was not found");
            }

            return currentNode;
        }

        public Node GetChildByName(string name) //дочерний элемент по имени
        {
            return Children?.Find(ch => ch.Name == name);
        }

        public void SortChildren() // сортировка дочерних элементов
        {
            var sortedList = Children.OrderBy(node => node.GetAttributeValue("Name")).ToList();
            Children.Clear();
            foreach (var node in sortedList)
            {
                Children.Add(node);
            }

        }

        #region Clone&ToStr

        public object Clone()
        {
           return this.MemberwiseClone();
        }

        public override string ToString()
        {
            var nodeString = new NodeXmlString();
            return nodeString.DrawTree(this);
        }

        #endregion

        #region Enumerator
        public IEnumerator<Node> GetEnumerator()
        {
            yield return this;
            foreach (var node in Children)
            {
                foreach (var child in node)
                    yield return child;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

    }
}