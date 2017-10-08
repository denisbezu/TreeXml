using System.Collections.Generic;

namespace TreeXmlLibrary.Search
{
    public abstract class SearchHelper
    {

        protected void AddFoundNode(ref List<Node> foundNodes, Node currentNode)
        {
            if (foundNodes == null)
                foundNodes = new List<Node>();
            foundNodes.Add(currentNode);
        }

    }
}