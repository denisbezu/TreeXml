using System.Collections.Generic;

namespace TreeXmlLibrary
{
    public class Tree<T> where T:class
    {
        private bool _rootExist;
        private List<T> Nodes { get; set; }
        public Tree()
        {
            Nodes = new List<T>();
        }
        public Node<T> AddRoot(T t)
        {
            var root = new Node<T>(t);
            if (!_rootExist)
            {
                Nodes.Add(t);
                _rootExist = true;
            }
            return root;
        }
        public Node<T> AddNode(T t, Node<T> parent)// поправить
        {
            var addedNode = new Node<T>(t);
            if (!CheckContains(addedNode, parent))
            {
                addedNode.Parent = parent;
                parent.Children.Add(addedNode);
            }
            else
            {
                addedNode = null;
            }
            return addedNode;
        }
        private bool CheckContains(Node<T> addedNode, Node<T> parent)
        {
            if (parent.Children.Contains(addedNode) || Nodes.Contains(addedNode.Value))
                return true;
            Nodes.Add(addedNode.Value);
            return false;
        }
    }
}