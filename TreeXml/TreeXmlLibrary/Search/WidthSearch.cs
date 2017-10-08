using System;
using System.Collections.Generic;
using TreeXmlLibrary.Enums;
using TreeXmlLibrary.interfaces;

namespace TreeXmlLibrary.Search
{
    public class WidthSearch : SearchHelper, ISearch
    {
      public List<Node> ExecuteSearch(Node rootNode, Predicate<Node> equals, out int step, SearchMode searchMode)
        {
            var nodesQueue = new Queue<Node>();
            List<Node> foundNodes = null;
            nodesQueue.Enqueue(rootNode);
            step = 0;
            while (nodesQueue.Count != 0)
            {
                step++;
                var currentNode = nodesQueue.Peek();
                if (equals(currentNode))
                {
                    AddFoundNode(ref foundNodes, currentNode);
                    if (searchMode == SearchMode.First)
                        return foundNodes;
                }
                nodesQueue.Dequeue();
                foreach (var children in currentNode.Children)
                    nodesQueue.Enqueue(children);
            }
            return foundNodes;
        }
    }
}