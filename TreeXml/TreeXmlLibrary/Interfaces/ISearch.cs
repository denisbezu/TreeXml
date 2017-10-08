using System;
using System.Collections.Generic;
using TreeXmlLibrary.Enums;

namespace TreeXmlLibrary.interfaces
{
    public interface ISearch
    {
        List<Node> ExecuteSearch(Node rootNode, Predicate<Node> equals, out int step, SearchMode searchMode);
    }
}