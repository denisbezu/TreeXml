using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TreeXmlLibrary 
{
    public class Node<T> where T : class
    {
        private readonly Lazy<List<Node<T>>> _children;
        public Node(T t)
        {
            _children = new Lazy<List<Node<T>>>(() => new List<Node<T>>());
            Instance = t;
        }
        //public static Node<T> AddRoot(T t, Tree<T> tree)
        //{
        //    var root = new Node<T>(t);
        //    tree.Nodes.Add(t);
        //    return root;
        //}

        //public Node<T> AddNode(T t, Tree<T> tree)
        //{
        //    var addedNode = new Node<T>(t);
        //    if (!CheckContains(addedNode, tree))
        //    {
        //        addedNode.Parent = this;
        //        Children.Value.Add(addedNode);
        //    }
        //    else
        //    {
        //       addedNode = null;
        //    }
        //    return addedNode;
        //}
        private Node<T> _parent;
        //private bool CheckContains(Node<T> addedNode, Tree<T> tree)
        //{
        //    if (Children.Value.Contains(addedNode) || tree.Nodes.Contains(addedNode.Instance))
        //        return true;
        //    tree.Nodes.Add(addedNode.Instance);
        //    return false;
        //}
        public Node<T> Parent
        {
            get { return _parent; }
            set
            {
                if (Parent != null || value == null)
                {
                    Parent.Children.Remove(this);
                }
                //else
                //{
                //    value.Children.Add(this);
                //}
                _parent = value;
            }
        }
        public T Instance { get; set; }
        public List<Node<T>> Children
        {
            get { return _children.Value; }
        }
        #region Equals
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals(((Node<T>)obj).Instance);
        }

        public bool Equals(T t)
        {
            return Instance.Equals(t);
        }
        #endregion
    }
}