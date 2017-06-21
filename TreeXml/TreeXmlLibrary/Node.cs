using System;
using System.Collections.ObjectModel;
using System.Text;

namespace TreeXmlLibrary
{
    public class Node<T> where T : class
    {
        public Node()
        {
            _children = new NodeCollection(this);
        }

        public Node(T t)
        {
            _children = new NodeCollection(this);
            this.Temp = t;
        }

        public Node<T> AddNode(T t)
        {
            var addedNode = new Node<T>(t);
            addedNode.Parent = this;
            this.Children.Add(addedNode);
            return addedNode;
        }

        Node<T> _parent;

        public Node<T> Parent
        {
            get { return _parent; }
            set
            {
                if (Parent != null || value == null)
                {
                    Parent.Children.Remove(this);
                }
                else
                {
                    value.Children.Add(this);
                }
                _parent = value;
            }
        }

        public T Temp { get; set; }

        #region Equals

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals(((Node<T>)obj).Temp);
        }

        public bool Equals(T t)
        {
            return Temp.Equals(t);
        }

        #endregion

        public int Level
        {
            get { return Parent != null ? this.Parent.Level + 1 : 0; }
        }

        #region ConsoleDrawing
        public override string ToString()
        {
            var rv = new StringBuilder(Temp.ToString());
            foreach (Node<T> ch in Children)
            {
                SubNodeToString(ch, rv);
            }
            return rv.ToString();
        }

        private void SubNodeToString(Node<T> n, StringBuilder sb)
        {
            sb.Append("\n" + repeat("\t", n.Level));
            sb.Append(n.Temp);
            sb.Append(string.Format(" (Parent: {0})", n.Parent.Temp));
            foreach (Node<T> ch in n.Children)
            {
                SubNodeToString(ch, sb);
            }
        }

        private string repeat(string s, int count)
        {
            var rv = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                rv.Append(s);
            }
            ;
            return rv.ToString();
        }
        #endregion
        
        NodeCollection _children;
        public NodeCollection Children
        {
            get { return _children; }
        }

        public class NodeCollection : Collection<Node<T>>
        {
            internal NodeCollection(Node<T> owner)
            {
                _owner = owner;
            }
            Node<T> _owner;
            protected override void InsertItem(int index, Node<T> item)
            {
                try
                {
                    if (item.Parent == null)
                    {
                        throw new Exception();
                    }
                    if (!this.Contains(item) && !AllNodes<T>.Nodes.Contains(item.Temp))
                    {
                        base.InsertItem(index, item);
                        item._parent = _owner;
                        AllNodes<T>.Nodes.Add(item.Temp);
                    }
                    else
                    {
                        item.Parent = null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Impossible to add item = " + item.Temp);
                }
            }
            protected override void RemoveItem(int index)
            {
                this[index]._parent = null;
                base.RemoveItem(index);
            }
        }
    }
}