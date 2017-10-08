using System;
using TreeXmlLibrary;

namespace TreeWPF.Helpers.SaveOpen
{
    public class NodeViewModelMomento
    {
        public string NodeName { get; set; }

        public Node Node { get; set; }

        public string Type { get; set; }

        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }

        public Action<object> OnSelectedAction { get; set; }

        public NodeViewModelMomento(string nodeName, Node node, string type, bool isSelected, bool isExpanded, Action<object> onSelectedAction) : this(nodeName, node, type, isSelected, isExpanded)
        {
            OnSelectedAction = onSelectedAction;
        }
        public NodeViewModelMomento(string nodeName, Node node, string type, bool isSelected, bool isExpanded)
        {
            NodeName = nodeName;
            Node = node;
            Type = type;
            IsSelected = isSelected;
            IsExpanded = isExpanded;
        }
    }
}