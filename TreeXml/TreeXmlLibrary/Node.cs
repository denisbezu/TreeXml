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
            Value = t;
        }
        private Node<T> _parent;
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
        public T Value { get; set; }
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
            return Equals(((Node<T>)obj).Value);
        }

        public bool Equals(T t)
        {
            return Value.Equals(t);
        }
        #endregion
    }
}