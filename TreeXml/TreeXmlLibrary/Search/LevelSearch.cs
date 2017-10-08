using System;
using System.Collections.Generic;
using TreeXmlLibrary.Enums;
using TreeXmlLibrary.interfaces;

namespace TreeXmlLibrary.Search
{
    public class LevelSearch : SearchHelper, ISearch
    {
        public List<Node> ExecuteSearch(Node rootNode, Predicate<Node> equals, out int step, SearchMode searchMode)
        {
            var nodesStack = new Stack<Node>();
            List<Node> foundNodes = null;
            nodesStack.Push(rootNode);
            step = 0;
            while (nodesStack.Count != 0)
            {
                step++;
                var currentNode = nodesStack.Pop();
                if (equals(currentNode))
                {
                    AddFoundNode(ref foundNodes, currentNode);
                    if (searchMode == SearchMode.First)
                        return foundNodes;
                }
                foreach (var children in currentNode.Children)
                    nodesStack.Push(children);
            }
            return foundNodes;
        }
    }
}