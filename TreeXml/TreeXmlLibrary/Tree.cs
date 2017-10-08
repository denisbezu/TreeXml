using System;
using System.Collections.Generic;

namespace TreeXmlLibrary
{
    public class Tree
    {
        public List<Node> Nodes { get; private set; }

        public string Name { get; set; }

        public Node Root { get; private set; }

        public Tree() : this(new Node())
        { }
        
        public Tree(Node root)
        {
            SetRoot(root);
        }

        private void SetRoot(Node root) // Установка корня дерева
        {
            if (Root != null)
                throw new Exception("The root item is already set, you cannot change it!");
            if(root.Tree != null)
                throw new Exception("This node has already a tree");
            Root = root;
            Root.Tree = this;
            Name = Root.Name;
            Nodes = new List<Node> {Root};
        }

        public override string ToString()
        {
            return Name;
        }
    }
}